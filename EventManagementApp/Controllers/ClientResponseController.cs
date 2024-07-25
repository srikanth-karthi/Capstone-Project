using System.Diagnostics.CodeAnalysis;
using EventManagementApp.DTOs.ClientResponse;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Controllers
{
    [Route("api/user/response")]
    [ApiController]
    [Authorize(Roles = "User")]
    [ExcludeFromCodeCoverage]

    public class ClientResponseController : ControllerBase
    {
        private readonly IClientResponseService _clientResponseService;

        public ClientResponseController(IClientResponseService clientResponseService)
        {
            _clientResponseService = clientResponseService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientResponse([FromBody] ClientResponseDTO clientResponseDTO)
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
                ClientResponse clientResponse = await _clientResponseService.CreateClientResponse(clientResponseDTO, UserId);

                if (clientResponse.ClientDecision == ClientDecision.Accepted)
                {
                    return Ok(new
                    {
                        Message = "Order created successfully",
                        clientResponse.Order.OrderId
                    });
                }

                return Ok(new
                {
                    Message = "User Decision added successfully"
                });

            }
            catch (RequestNotApprovedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NoQuotationResponseFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ClientAlreadyRespondedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

    }
}
