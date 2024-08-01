using System.Diagnostics.CodeAnalysis;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.DTOs.ReviewDTO;
using EventManagementApp.DTOs.ScheduledEvent;
using EventManagementApp.DTOs.User;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Controllers
{
    [Route("api/user/")]
    [ApiController]
    [Authorize(Roles = "User")]
    [ExcludeFromCodeCoverage]

    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("requests")]
        [HttpGet]
        public async Task<IActionResult> GetUserRequests()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());

                List<BasicQuotationRequestDTO> requests = await _userService.GetUserRequests(UserId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [Route("profile")]
        [HttpGet]
        public async Task<IActionResult> GetUserprofile()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());
                    var profile = await _userService.GetUserProfile(UserId);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [Route("requests/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRequests(int id)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());

                UserQuotationRequestDTO requests = await _userService.GetUserRequestById(UserId, id);
                return Ok(requests);
            }
            catch(NoQuotationRequestFoundException)
            {
                return NotFound("No request found for a given Id");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [Route("orders")]
        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());

                List<UserOrderListReturnDTO> requests = await _userService.GetUserOrders(UserId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [Route("eventCategory")]
        [HttpGet]
        public async Task<IActionResult> GetUserEvents()
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());

                List<BasicScheduledEventListDTO> requests = await _userService.GetUserEvents(UserId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [Route("orders/reviews/{OrderId}")]
        [HttpPost]
        public async Task<IActionResult> ReviewAnOrder(int OrderId, [FromBody] ReviewDTO ReviewDTO)
        {
            try
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

                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());

                await _userService.ReviewAnOrder(UserId, OrderId, ReviewDTO);
                var response = new
                {
                    Status = "Success",
                    Message = "Your review is added successfully"
                };

                return Ok(response);
            }
            catch (NoOrderFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (OrderReviewedAlreadyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(EventNotCompletedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PaymentNotCompletedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

    }
}
