using System.Collections.Generic;
using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.DTOs.ReviewDTO;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;


namespace EventManagementApp.Services
{
    public class EventCategoryService : IEventCategoryService
    {
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public EventCategoryService(IEventCategoryRepository eventCategoryRepository)
        {
            _eventCategoryRepository = eventCategoryRepository;
        }
        

        public async Task<List<BaseEventCategoryDTO>> GetAllEventCategories()
        {
            var events = await _eventCategoryRepository.GetAllActiveWithReviews();
            return events;
        }

        public async Task<List<UserReviewDTO>> GetTopReviews()
        {
            var events = await _eventCategoryRepository.TopReviews();
            return events;
        }

    }
}
