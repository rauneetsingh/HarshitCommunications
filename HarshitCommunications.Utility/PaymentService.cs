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
    public class PaymentService : IPaymentService
    {
        private readonly string _keyId;
        private readonly string _keySecret;

        public PaymentService(IConfiguration configuration)
        {
            _keyId = configuration["Razorpay:KeyId"];
            _keySecret = configuration["Razorpay:KeySecret"];
        }

        public Order CreateOrder(decimal amount)
        {
            var client = new RazorpayClient(_keyId, _keySecret);

            var options = new Dictionary<string, object>
            {
                { "amount", amount * 100 },
                { "currency", "INR" },
                { "payment_capture", "1" }
            };

            return client.Order.Create(options);
        }

        public bool ValidateSignature(string orderId, string paymentId, string signature)
        {
            string data = orderId + "|" + paymentId;
            string generatedSignature = GenerateSignature(data, _keySecret);
            return generatedSignature == signature;
        }

        private string GenerateSignature(string data, string key)
        {
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(data);

            using (var hmacsha256 = new System.Security.Cryptography.HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            }
        }
    }
}
