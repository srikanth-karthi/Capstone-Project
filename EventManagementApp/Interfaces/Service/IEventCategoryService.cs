using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.DTOs.ReviewDTO;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Service
{
    public interface IEventCategoryService
    {
        public Task<List<BaseEventCategoryDTO>> GetAllEventCategories();
        public Task<List<UserReviewDTO>> GetTopReviews();

    }
}
