using EventManagementApp.DTOs.Event;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // POST api/ticket/book
        [HttpPost("book")]
        public async Task<IActionResult> BookTicket([FromBody] TicketDTO ticketDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var customErrorResponse = new
                {
                    Title = "One or more validation errors occurred.",
                    Errors = errors
                };

                return BadRequest(customErrorResponse);
            }

            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());
                var response = await _ticketService.BookTicket(ticketDto, UserId);
                return Ok(response);
            }
            catch (Exception ex)
            {
   
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetTickets")]
        public async Task<IActionResult> GetTickets()
        {

            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());
                var response = await _ticketService.GetTicketsForUser(UserId);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmTicket([FromBody] ConfirmPaymentDTO confirmTicketDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var customErrorResponse = new
                {
                    Title = "One or more validation errors occurred.",
                    Errors = errors
                };

                return BadRequest(customErrorResponse);
            }

            try
            {
                var ticket = await _ticketService.ConfirmTicket(
                    confirmTicketDto
                );

                return Ok(ticket);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [AllowAnonymous]
        [HttpGet("checkin/{TicketId}")]
        public async Task<IActionResult> CheckInTicket(string TicketId)
        {


            try
            {
                var ticket = await _ticketService.CheckInTicket(TicketId, 1);

                return Ok(new { message = $"One ticket checkin,{ticket.CheckedInTickets} avaliable " });
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
