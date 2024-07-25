using EventManagementApp.DTOs.NotificationDTOs;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using EventManagementApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace EventManagementApp.Controllers
{

    [Route("api/notifications")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
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

                var notifications = await _notificationService.GetAllNotifications(UserId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

       
    }
}