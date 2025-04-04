using HarshitCommunications.DataAccess.Repository.IRepository;
using HarshitCommunications.Models;
using HarshitCommunications.Models.ViewModels;
using HarshitCommunications.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HarshitCommunications.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        private ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.Address = ShoppingCartVM.OrderHeader.ApplicationUser.Address;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.Pincode = ShoppingCartVM.OrderHeader.ApplicationUser.Pincode;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST([Bind("OrderHeader")] ShoppingCartVM shoppingCartVM, string paymentType)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (shoppingCartVM == null)
            {
                shoppingCartVM = new ShoppingCartVM();
            }

            if (shoppingCartVM.OrderHeader == null)
            {
                shoppingCartVM.OrderHeader = new OrderHeader();
            }

            shoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == userId, includeProperties: "Product")?.ToList();

            if (shoppingCartVM.ShoppingCartList == null || !shoppingCartVM.ShoppingCartList.Any())
            {
                ModelState.AddModelError("", "Your cart is empty. Add items before proceeding.");
                return View(shoppingCartVM);
            }

            shoppingCartVM.OrderHeader.ApplicationUserId = userId;
            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            _unitOfWork.OrderHeader.Add(shoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = shoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
            }
            _unitOfWork.Save();

            if (paymentType == "PayNow")
            {
                // Create Razorpay Order
                var order = PaymentService.CreateOrder((decimal)shoppingCartVM.OrderHeader.OrderTotal);
                string razorpayOrderId = order["id"].ToString();

                _unitOfWork.OrderHeader.UpdateRazorpayOrderId(
                    shoppingCartVM.OrderHeader.Id,
                    RazorpayOrderId: razorpayOrderId
                );
                _unitOfWork.Save();

                return RedirectToAction(nameof(Payment), new { id = shoppingCartVM.OrderHeader.Id });
            }
            else if (paymentType == "CashOnDelivery")
            {
                // Update order as Approved for COD
                _unitOfWork.OrderHeader.UpdateStatus(
                    shoppingCartVM.OrderHeader.Id,
                    SD.StatusApproved,
                    SD.PaymentStatusApproved
                );

                // Clear shopping cart
                var shoppingCartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId);
                if (shoppingCartItems != null && shoppingCartItems.Any())
                {
                    _unitOfWork.ShoppingCart.RemoveRange(shoppingCartItems);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(CODOrderSuccess), new { id = shoppingCartVM.OrderHeader.Id });
            }

            return RedirectToAction(nameof(Index), "Cart");

        }

        [HttpGet]
        public IActionResult CashOnDelivery(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            if (orderHeader == null)
            {
                return NotFound();
            }

            // Update order statuses to Approved
            orderHeader.PaymentStatus = SD.PaymentStatusApproved;
            orderHeader.OrderStatus = SD.StatusApproved;
            _unitOfWork.Save();

            return RedirectToAction(nameof(CODOrderSuccess), new { id = orderHeader.Id });
        }

        [HttpGet]
        public IActionResult CODOrderSuccess(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            if (orderHeader == null)
            {
                return NotFound();
            }

            ViewBag.OrderId = orderHeader.PaymentIntentId;

            return View(orderHeader); // Pass the order details to the view
        }

        public IActionResult Payment(int id)
        {
            // Fetch the order details
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            if (orderHeader == null)
            {
                return NotFound(); // Return 404 if the order is not found
            }

            // Check if Razorpay Order ID exists; create one if missing
            if (string.IsNullOrEmpty(orderHeader.RazorpayOrderId))
            {
                // Create Razorpay Order
                var razorpayOrder = PaymentService.CreateOrder((decimal)orderHeader.OrderTotal);
                orderHeader.RazorpayOrderId = razorpayOrder["id"].ToString(); // Save Razorpay Order ID
                _unitOfWork.OrderHeader.Update(orderHeader);
                _unitOfWork.Save();
            }

            // Pass data to the payment view using ViewBag
            ViewBag.OrderId = orderHeader.RazorpayOrderId; // Razorpay Order ID
            ViewBag.Amount = (orderHeader.OrderTotal * 100).ToString("F0"); // Amount in paise
            ViewBag.CustomerEmail = orderHeader.ApplicationUser.Email;
            ViewBag.CustomerName = orderHeader.ApplicationUser.Name;

            return View();
        }

        public static class RazorpaySignatureValidator
        {
            public static bool Validate(string orderId, string paymentId, string razorpaySignature, string keySecret)
            {
                // Concatenate order ID and payment ID
                string payload = orderId + "|" + paymentId;

                // Generate hash using HMACSHA256 with your Razorpay Key Secret
                using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(keySecret)))
                {
                    byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                    string generatedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    // Compare Razorpay's signature with the generated signature
                    return generatedSignature == razorpaySignature;
                }
            }
        }

        [HttpPost]
        public IActionResult PaymentConfirmation(string razorpayPaymentId, string razorpayOrderId, string razorpaySignature)
        {
            // Validate Razorpay signature (optional but recommended)
            bool isValid = RazorpaySignatureValidator.Validate(razorpayOrderId, razorpayPaymentId, razorpaySignature, PaymentService.keySecret);

            if (!isValid)
            {
                return Json(new { success = false, message = "Payment validation failed!" });
            }

            // Fetch the order based on the Razorpay Order ID
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.RazorpayOrderId == razorpayOrderId, includeProperties: "ApplicationUser");
            if (orderHeader == null)
            {
                return Json(new { success = false, message = "Order not found!" });
            }

            // Update the order with the Payment ID and approve payment
            orderHeader.PaymentIntentId = razorpayPaymentId; // Save Razorpay Payment ID
            orderHeader.PaymentStatus = SD.PaymentStatusApproved;
            orderHeader.OrderStatus = SD.StatusPending; // Leave status as pending until OrderConfirmation

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Payment validated successfully!", orderId = orderHeader.Id });
        }

        [HttpGet]
        public IActionResult OrderConfirmation(string id)
        {
            // Validate PaymentIntentId (Razorpay Payment ID)
            if (string.IsNullOrEmpty(id))
            {
                return Content("Invalid Payment ID.");
            }

            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.RazorpayOrderId == id, includeProperties: "ApplicationUser");
            if (orderHeader == null)
            {
                return Content("Order not found.");
            }

            // Approve the order
            _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusApproved, SD.PaymentStatusApproved);

            // Clear the shopping cart
            var shoppingCartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId);
            if (shoppingCartItems != null && shoppingCartItems.Any())
            {
                _unitOfWork.ShoppingCart.RemoveRange(shoppingCartItems);
            }

            _unitOfWork.Save();

            // Redirect to an Order Summary page for the customer
            return RedirectToAction("Index", "Cart", new { id = orderHeader.Id });
        }


        public IActionResult PaymentCancelled()
        {
            return View();
        }


        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);

            if (cartFromDb.Count <= 1)
            {
                //remove from cart
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);

                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);

            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            return shoppingCart.Product.Price;
        }
    }
}
