using HarshitCommunications.DataAccess.Data;
using HarshitCommunications.DataAccess.Repository.IRepository;
using HarshitCommunications.Models;
using HarshitCommunications.Utility;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarshitCommunications.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateRazorpayOrderId(int id, string RazorpayOrderId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);

            if (!string.IsNullOrEmpty(RazorpayOrderId))
            {
                orderFromDb.RazorpayOrderId = RazorpayOrderId;
                orderFromDb.PaymentDate = DateTime.Now;
            }
        }

        public bool RefundPayment(int orderId)
        {
            var orderHeader = _db.OrderHeaders.FirstOrDefault(o => o.Id == orderId);
            if (orderHeader == null || string.IsNullOrEmpty(orderHeader.PaymentIntentId))
            {
                return false; // Order not found or PaymentIntentId is missing
            }

            var client = new RestClient("https://api.razorpay.com/v1/payments/" + orderHeader.PaymentIntentId + "/refund");
            var request = new RestRequest { Method = Method.Post };

            // Razorpay API Key & Secret (Replace with actual credentials)
            string apiKey = "rzp_test_67XFtDb0zvHVVa";
            string apiSecret = "CZBY8a0YpoBUadU9Ufa0fjDs";
            string encodedAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));
            request.AddHeader("Authorization", $"Basic {encodedAuth}");

            // Razorpay refund parameters
            var body = new
            {
                amount = (orderHeader.OrderTotal * 100) // Convert to paise
            };

            request.AddJsonBody(body);

            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                orderHeader.OrderStatus = SD.StatusRefunded;
                orderHeader.PaymentStatus = SD.StatusRefunded;

                return true;
            }

            return false;
        }

        public decimal GetTotalRevenue()
        {
            return _db.OrderHeaders
                .Where(o => o.PaymentStatus != SD.StatusRefunded)
                .Sum(o => (decimal?)o.OrderTotal) ?? 0;
        }

        public int GetTotalProductsSold()
        {
            var orderIds = _db.OrderHeaders
                .Where(o => o.PaymentStatus != SD.StatusRefunded)
                .Select(o => o.Id);

            return _db.OrderDetails
                .Where(od => orderIds.Contains(od.OrderHeaderId))
                .Sum(od => (int?)od.Count) ?? 0;
        }
    }
}