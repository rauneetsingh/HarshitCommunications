using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Razorpay.Api;

namespace HarshitCommunications.Utility
{
    public class PaymentService
    {
        private const string keyId = "rzp_test_67XFtDb0zvHVVa";
        private const string keySecret = "CZBY8a0YpoBUadU9Ufa0fjDs";

        public static Order CreateOrder(decimal amount)
        {
            RazorpayClient client = new RazorpayClient(keyId, keySecret);

            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", amount * 100 }, // Convert to paisa
                { "currency", "INR" },
                { "payment_capture", "1" } // Auto capture payment
            };

            return client.Order.Create(options);
        }
    }
}
