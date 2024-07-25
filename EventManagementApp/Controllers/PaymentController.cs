// Controllers/PaymentController.cs
using EventManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Razorpay.Api;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class PaymentController : ControllerBase
{
    private readonly string key = "rzp_test_kHemgfwkQxlJk2";
    private readonly string secret = "YlOgAzCsLjNW0SwfK4AiqCpZ";

    [HttpPost("order")]
    public IActionResult CreateOrder()
    {
        int price = 100;
        Dictionary<string, object> input = new Dictionary<string, object>();
        input.Add("amount", (int)price * 100);
        input.Add("currency", "INR");
        input.Add("receipt", Guid.NewGuid().ToString());

        RazorpayClient client = new RazorpayClient(key, secret);

        Razorpay.Api.Order order = client.Order.Create(input);
        string orderId = order["id"].ToString();
return Ok(orderId);
    }
}
