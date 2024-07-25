using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Controllers
{
    [Route("api/user/event")]
    [ApiController]
    [ExcludeFromCodeCoverage]

    public class ScheduledEventController : ControllerBase
    {
        private readonly IScheduledEventService _scheduledEventService;

        public ScheduledEventController(IScheduledEventService scheduledEventService)
        {
            _scheduledEventService = scheduledEventService;
        }

        [HttpPut]
        [Route("{eventId}")]
        public async Task<IActionResult> MarkScheduledEventComplete(int eventId)
        {
            try
            {
                int UserId = int.Parse(User.FindFirst("userId").Value.ToString());
                UserType userType = User.FindFirst(ClaimTypes.Role).Value.ToString() == "Admin" ? UserType.Admin : UserType.User;

                if (userType == UserType.User)
                {
                    await _scheduledEventService.MarkEventAsCompleted(eventId, UserId);
                }
                else
                {
                    await _scheduledEventService.MarkEventAsCompleted(eventId);
                }

                return Ok("Schedule is marked as Completed");
            }
            catch(NoScheduledEventFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

    }
}
