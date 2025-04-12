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
    }
}
