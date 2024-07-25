using System.Diagnostics.CodeAnalysis;
using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.DTOs.ReviewDTO;
using EventManagementApp.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Controllers
{
    [Route("api/eventCategory")]
    [ApiController]
    [ExcludeFromCodeCoverage]

    public class EventCategoryController: ControllerBase
    {
        private readonly IEventCategoryService _eventCategoryService;

        public EventCategoryController(IEventCategoryService eventCategoryService) {
            _eventCategoryService = eventCategoryService;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllEventCategories()
        {
            try
            {

                List<BaseEventCategoryDTO> events = await _eventCategoryService.GetAllEventCategories();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("review/top")]
        public async Task<IActionResult> GetTopReviews()
        {
            try
            {
                List<UserReviewDTO> reviews = await _eventCategoryService.GetTopReviews();
                return Ok(reviews);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

    }
}
