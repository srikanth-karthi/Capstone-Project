using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManagementApp.DTOs.Event;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.DTOs;
using EventManagementApp.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace EventManagementApp.Controllers
{
    [ApiController]
    [Route("api/events")]

    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Admin")]
        public async Task<IActionResult> GetAllEvents()
        {
            try
            {
                var events = await _eventService.GetAllEvents();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving events.", details = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetEventsForUser()
        {
            try
            {
                var events = await _eventService.GetEventsForUser();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving events.", details = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{eventId}/tickets")]
        public async Task<IActionResult> GetTicketsForEvent(int eventId)
        {
            try
            {
                var tickets = await _eventService.GetTicketsForEvent(eventId);
                return Ok(tickets);
            }
            catch (EventNotFoundExceptions ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving tickets.", details = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateEvent(int eventId, [FromForm] UpdateEventDto eventDto)
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
                var updatedEvent = await _eventService.UpdateEvent(eventDto, eventId);
                return Ok(updatedEvent);
            }
            catch (EventNotFoundExceptions ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidTicketpriceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the event.", details = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddEvent([FromForm] AddEventDto eventDto)
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
                var newEvent = await _eventService.AddEvent(eventDto);
                return CreatedAtAction(nameof(GetTicketsForEvent), new { eventId = newEvent.EventId }, newEvent);
            }
            catch (InvalidTicketpriceException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the event.", details = ex.Message });
            }
        }
    }
}
