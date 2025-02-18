using Razorpay.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RazorpayIntegration.Services
{
    public class RazorpayService
    {
        private readonly string _key;
        private readonly string _secret;

        public RazorpayService(string key, string secret)
        {
            _key = key;
            _secret = secret;
        }

        public async Task<Order> CreateOrderAsync(decimal amount, string currency, string receiptId)
        {
            RazorpayClient client = new RazorpayClient(_key, _secret);

            var options = new Dictionary<string, object>
            {
                { "amount", (int)(amount * 100) }, // Convert to paisa
                { "currency", currency },
                { "receipt", receiptId }, // Receipt ID comes from frontend
                { "payment_capture", 1 }
            };

            return await Task.Run(() => client.Order.Create(options));
        }

        public async Task<Payment> FetchPaymentDetailsAsync(string paymentId)
        {
            RazorpayClient client = new RazorpayClient(_key, _secret);
            return await Task.Run(() => client.Payment.Fetch(paymentId));
        }
    }
}
