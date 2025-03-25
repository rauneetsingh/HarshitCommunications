using System.Diagnostics;
using System.Security.Claims;
using HarshitCommunications.DataAccess.Repository.IRepository;
using HarshitCommunications.Models;
using HarshitCommunications.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HarshitCommunications.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
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
            //if (!ModelState.IsValid)
            //{
            //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        Console.WriteLine($"Model Binding Error: {error.ErrorMessage}");
            //    }
            //}

            //Console.WriteLine($"Received ProductId: {shoppingCart.ProductId}");
            //Console.WriteLine($"Received Count: {shoppingCart.Count}");  // Debugging count value

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(
                u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

            if(cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            else
            {
                //add to cart
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }

            TempData["success"] = "Cart Updated successfully!";

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
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
    }
}
