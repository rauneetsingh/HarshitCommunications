using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;

namespace HarshitCommunications.Utility
{
    public interface IPaymentService
    {
        Order CreateOrder(decimal amount);
        bool ValidateSignature(string orderId, string paymentId, string signature);
    }
}
