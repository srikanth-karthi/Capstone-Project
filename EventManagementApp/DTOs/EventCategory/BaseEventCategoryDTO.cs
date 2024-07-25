using EventManagementApp.DTOs.ReviewDTO;

namespace EventManagementApp.DTOs.EventCategory
{
    public class BaseEventCategoryDTO
    {
        public int EventCategoryId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public float? Rating { get; set; }
        public List<UserReviewDTO> Reviews { get; set; }
    }
}
