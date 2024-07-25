using System.Diagnostics.CodeAnalysis;
using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.DTOs.ScheduledEvent;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EventManagementApp.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ExcludeFromCodeCoverage]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost]
        [Route("eventCategory")]
        public async Task<IActionResult> CreateEventCategory(CreateEventCategoryDTO eventDTO)
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

                await _adminService.CreateEventCategory(eventDTO);
                return StatusCode(StatusCodes.Status201Created, "Event Category created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("eventCategory")]
        public async Task<IActionResult> GetAllEventCategories()
        {
            try
            {

                List<AdminBaseEventCategoryDTO> events = await _adminService.GetAllEventCategories();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("eventCategory/scheduled")]
        public async Task<IActionResult> GetScheduledEvents()
        {
            try
            {
                List<AdminScheduledEventListDTO> scheduledEvents = await _adminService.GetScheduledEvents();
                return Ok(scheduledEvents);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPut]
        [Route("eventCategory/{id}")]
        public async Task<IActionResult> UpdateEventDetails(int id, [FromBody] UpdateEventCategoryDTO updateEventCategoryDTO)
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

                await _adminService.UpdateEventDetails(id, updateEventCategoryDTO);
                return Ok("Event Category has been Updated");
            }
            catch(NoEventCategoryFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(NullReferenceException ex)
            {
                return BadRequest("EventName or Description or IsActive is required");
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
        [HttpGet]
        [Route("quotations")]
        public async Task<IActionResult> GetQuotations(bool IsNew)
        {
            try
            {
                List<BasicQuotationRequestDTO> quotationRequestDTOs = await _adminService.GetQuotations(IsNew);
                return Ok(quotationRequestDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}
