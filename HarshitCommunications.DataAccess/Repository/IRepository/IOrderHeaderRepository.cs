using HarshitCommunications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarshitCommunications.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
        void UpdateRazorpayOrderId(int id, string RazorpayOrderId);
        bool RefundPayment(int orderId);
        decimal GetTotalRevenue();
        int GetTotalProductsSold();
    }
}
