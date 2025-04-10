using System.Diagnostics;
using System.Security.Claims;
using HarshitCommunications.DataAccess.Repository.IRepository;
using HarshitCommunications.Models;
using HarshitCommunications.Models.ViewModels;
using HarshitCommunications.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HarshitCommunications.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            IEnumerable<Category> categoryList = _unitOfWork.Category.GetAll();

            ViewBag.Categories = categoryList;

            return View(productList);
        }

        public IActionResult AllProducts()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            IEnumerable<Category> categoryList = _unitOfWork.Category.GetAll();

            ViewBag.Categories = categoryList;

            return View(productList);
        }

        [HttpGet]
        public IActionResult FilterProducts([FromQuery] int[] categoryIds, string sortOption)
        {
            Console.WriteLine("Category IDs received: " + string.Join(", ", categoryIds ?? new int[0]));
            Console.WriteLine("SortOption received: " + sortOption);

            // Fetch all products if no categories are selected
            var filteredProducts = _unitOfWork.Product.GetAll(includeProperties: "Category");

            // Apply filtering if categoryIds are provided
            if (categoryIds != null && categoryIds.Any())
            {
                filteredProducts = filteredProducts.Where(p => categoryIds.Contains(p.CategoryId));
            }

            // Apply sorting logic
            if (sortOption == "price-asc")
            {
                filteredProducts = filteredProducts.OrderBy(p => p.Price);
            }
            else if (sortOption == "price-desc")
            {
                filteredProducts = filteredProducts.OrderByDescending(p => p.Price);
            }

            // Return the filtered and/or sorted products
            return PartialView("_ProductGrid", filteredProducts);
        }

        public IActionResult Details(int productID)
        {
            var product = _unitOfWork.Product.Get(u => u.Id == productID, includeProperties: "Category");

            ShoppingCart cart = new ShoppingCart()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productID, includeProperties: "Category"),
                Count = 1,
                ProductId = productID
            };

            var relatedProducts = _unitOfWork.Product.GetAll(includeProperties: "Category")
                .Where(p => p.CategoryId == product.CategoryId && p.Id != productID)
                .Take(4)
                .ToList();

            var viewModel = new ProductDetailsVM
            {
                Cart = cart,
                RelatedProducts = relatedProducts
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(
                u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }

            else
            {
                //add to cart
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }

            TempData["success"] = "Cart Updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult MyOrdersList()
        {
            return View();
        }

        public IActionResult MyOrder(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };

            return View(OrderVM);
        }

        [HttpPost]
        public IActionResult RefundOrder(int orderId)
        {
            bool isRefunded = _unitOfWork.OrderHeader.RefundPayment(orderId);

            if (isRefunded)
            {
                var orderDetails = _unitOfWork.OrderDetail.GetAll(od => od.OrderHeaderId == orderId, includeProperties: "Product");

                foreach (var detail in orderDetails)
                {
                    var product = detail.Product;
                    if (product != null)
                    {
                        product.StockQuantity += detail.Count;
                        _unitOfWork.Product.Update(product);
                    }
                }

                _unitOfWork.Save();
                return Json(new { success = true, message = "Order refunded successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Refund failed. Please try again." });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<OrderHeader> objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser");

            Console.WriteLine($"Status Received: {status}");

            switch (status)
            {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusPending);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                case "refunded":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusRefunded);
                    break;
                default:
                    break;
            }
            return Json(new { data = objOrderHeaders });
        }

        #endregion
    }
}
