using System.Diagnostics;
using HarshitCommunications.DataAccess.Repository.IRepository;
using HarshitCommunications.Models;
using HarshitCommunications.Models.ViewModels;
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
            return View();
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
            Product product = _unitOfWork.Product.Get(u => u.Id == productID, includeProperties: "Category");

            var relatedProducts = _unitOfWork.Product.GetAll(includeProperties: "Category")
                .Where(p => p.CategoryId == product.CategoryId && p.Id != productID)
                .Take(4)
                .ToList();

            var viewModel = new ProductDetailsVM
            {
                Product = product,
                RelatedProducts = relatedProducts
            };

            return View(viewModel);
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
