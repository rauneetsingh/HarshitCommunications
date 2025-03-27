using HarshitCommunications.DataAccess.Repository.IRepository;
using HarshitCommunications.Utility;
using Microsoft.AspNetCore.Mvc;

namespace HarshitCommunications.Areas.Customer.Controllers
{

    [ApiController]
    [Route("api/payment")]
    public class PaymentWebhookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentWebhookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IActionResult RazorpayWebhook([FromBody] dynamic data)
        {
            string paymentStatus = data?.payload?.payment?.entity?.status;

            if (paymentStatus == "captured")
            {
                // Update Order Payment Status
                int orderId = int.Parse(data?.payload?.payment?.entity?.notes?.order_id);
                var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId);
                orderHeader.PaymentStatus = SD.PaymentStatusApproved;
                _unitOfWork.Save();
            }

            return Ok();
        }
    }
}
