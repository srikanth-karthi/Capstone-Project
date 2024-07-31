using EventManagementApp.DTOs.Event;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace EventManagementApp.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(Roles = "User")]
    [ExcludeFromCodeCoverage]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Orderpayment/{OrderId}")]
        public async Task<IActionResult> PayForOrder(int OrderId)
        {
      
            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());
                var paymentResponse = await _orderService.PayForOrder(OrderId, UserId);
                return Ok(paymentResponse);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Orderpayment/confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentDTO confirmPaymentDTO)
        {
            if (confirmPaymentDTO == null || string.IsNullOrEmpty(confirmPaymentDTO.PaymentId) ||
                string.IsNullOrEmpty(confirmPaymentDTO.ContentId) || string.IsNullOrEmpty(confirmPaymentDTO.Signature))
            {
                return BadRequest("Invalid input");
            }

            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());
                if (await _orderService.ConfirmPayment(confirmPaymentDTO, UserId))
                    return Ok(new { message = "Event created successfully" });
                else
                    return BadRequest("Payment confirmation failed.");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }


}
