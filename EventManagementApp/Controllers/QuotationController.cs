using System.Diagnostics.CodeAnalysis;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Controllers
{
    [Route("api/quotation")]
    [ApiController]
    [ExcludeFromCodeCoverage]

    public class QuotationController: ControllerBase
    {
        private readonly IQuotationRequestService _quotationRequestService;
        private readonly IQuotationResponseService _quotationResponseService;

        public QuotationController(IQuotationRequestService quotationRequestService, 
            IQuotationResponseService quotationResponseService
            ) 
        {
            _quotationRequestService = quotationRequestService;
            _quotationResponseService = quotationResponseService;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("request")]
        public async Task<IActionResult> CreateQuotationRequest(CreateQuotationRequestDTO createQuotationRequestDTO)
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
                int QuotationRequestId = await _quotationRequestService.CreateQuotationRequest(UserId, createQuotationRequestDTO);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Request created successfully",
                    QuotationRequestId
                });
            }
            catch(NoEventCategoryFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(EventInActiveException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("response")]
        public async Task<IActionResult> CreateQuotationResponse(CreateQuotationResponseDTO createQuotationResponseDTO)
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

                int QuotationResponseId = await _quotationResponseService.CreateQuotationResponse(createQuotationResponseDTO);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    Message = "Quotation Responded successfully",
                    QuotationResponseId
                });
            }
            catch (NoQuotationRequestFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (QuotationAlreadyRespondedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AmountNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CurrencyNullException ex)
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
