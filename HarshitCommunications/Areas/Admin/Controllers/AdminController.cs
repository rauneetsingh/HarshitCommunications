using HarshitCommunications.DataAccess.Repository.IRepository;
using HarshitCommunications.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HarshitCommunications.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var totalRevenue = _unitOfWork.OrderHeader.GetTotalRevenue();
            var totalSold = _unitOfWork.OrderHeader.GetTotalProductsSold();

            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalSold = totalSold;

            return View();
        }

        public IActionResult GetMonthlyRevenue()
        {
            var data = _unitOfWork.OrderHeader.GetAll()
                .Where(o => o.PaymentStatus != SD.StatusRefunded)
                .GroupBy(o => o.OrderDate.ToString("MMM yyyy"))
                .Select(g => new { Month = g.Key, Total = g.Sum(o => o.OrderTotal) })
                .OrderBy(x => x.Month)
                .ToList();

            return Json(data);
        }

        public IActionResult GetCategorySales()
        {
            var data = _unitOfWork.OrderDetail
                   .GetAll(includeProperties: "Product,Product.Category")
                   .GroupBy(od => od.Product.Category.Name)
                   .Select(g => new { Category = g.Key, Total = g.Sum(od => od.Count) })
                   .ToList();

            return Json(data);
        }
    }
}
