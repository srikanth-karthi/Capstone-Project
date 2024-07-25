using EventManagementApp.Context;
using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.DTOs.ReviewDTO;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventManagementApp.Repositories
{
    public class EventCategoryRepository : Repository<EventCategory, int>, IEventCategoryRepository
    {
        public EventCategoryRepository(EventManagementDBContext _context) : base(_context)
        {
        }

        public async Task<List<BaseEventCategoryDTO>> GetAllActiveWithReviews()
        {
            List<BaseEventCategoryDTO> eventCategories = await _context.EventCategories
                .Where(e => e.IsActive)
                .Select(ec => new BaseEventCategoryDTO
                {
                    EventCategoryId = ec.EventCategoryId,
                    EventName = ec.EventName,
                    Description = ec.Description,
                    CreatedDate = ec.CreatedDate,
                    Rating = ec.Rating,
                    Reviews = new List<UserReviewDTO>()
                })
                .ToListAsync();

            foreach (var eventCategory in eventCategories)
            {
                eventCategory.Reviews = await _context.Reviews
                    .Where(r => r.EventCategoryId == eventCategory.EventCategoryId)
                    .Select(r => new UserReviewDTO
                    {
                        Comments = r.Comments,
                        Rating = r.Rating,
                        UserName = r.User.FullName
                    })
                    .ToListAsync();
            }

            return eventCategories;

        }

        public async Task<List<AdminBaseEventCategoryDTO>> GetAllWithReviews()
        {
            List<AdminBaseEventCategoryDTO> eventCategories = await _context.EventCategories
                .Select(ec => new AdminBaseEventCategoryDTO
                {
                    EventCategoryId = ec.EventCategoryId,
                    EventName = ec.EventName,
                    Description = ec.Description,
                    CreatedDate = ec.CreatedDate,
                    Rating = ec.Rating,
                    IsActive = ec.IsActive,
                    Reviews = new List<UserReviewDTO>()
                })
                .ToListAsync();

            foreach (var eventCategory in eventCategories)
            {
                eventCategory.Reviews = await _context.Reviews
                    .Where(r => r.EventCategoryId == eventCategory.EventCategoryId)
                    .Select(r => new UserReviewDTO
                    {
                        Comments = r.Comments,
                        Rating = r.Rating,
                        UserName = r.User.FullName
                    })
                    .ToListAsync();
            }

            return eventCategories;
        }

        public async Task<List<UserReviewDTO>> TopReviews()
        {
            List<UserReviewDTO> topReviews = await _context.Reviews
                .Select(review=> new UserReviewDTO
                {
                    Comments=review.Comments,
                    Rating=review.Rating,
                    UserName = review.User.FullName
                })
                .OrderByDescending(review=> review.Rating)
                .Take(5)
                .ToListAsync();

            return topReviews;
        }
    }
}
