using EventManagementApp.Interfaces.Service;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class PaymentService : IPaymentService
{
    private readonly string _key;
    private readonly string _secret;

    public PaymentService(IConfiguration configuration)
    {
        _key = configuration["Razorpay:Key"];
        _secret = configuration["Razorpay:Secret"];
    }

    public async Task<string> CreateOrder(decimal amount)
    {
        var client = new RazorpayClient(_key, _secret);

        var input = new Dictionary<string, object>
        {
            { "amount", amount * 100 }, 
            { "currency", "INR" },
            { "receipt", Guid.NewGuid().ToString() }
        };

        var order = await Task.Run(() => client.Order.Create(input));
        return order["id"].ToString();
    }

    public async Task<bool> VerifyPayment(string paymentId, string orderId, string signature)
    {
        var generatedSignature = GenerateSignature(paymentId, orderId);
        return generatedSignature == signature;
    }

    private string GenerateSignature(string paymentId, string orderId)
    {
        var body = orderId + "|" + paymentId;
        var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
