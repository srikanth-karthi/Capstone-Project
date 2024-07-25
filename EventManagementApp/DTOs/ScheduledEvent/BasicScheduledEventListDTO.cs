using EventManagementApp.Enums;

namespace EventManagementApp.DTOs.ScheduledEvent
{
    public class BasicScheduledEventListDTO
    {
        public int ScheduledEventId { get; set; }
        public string EventCategory { get; set; }
        public VenueType VenueType { get; set; }
        public string LocationDetails { get; set; }
        public FoodPreference FoodPreference { get; set; }
        public string? CateringInstructions { get; set; }
        public string SpecialInstructions { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsCompleted { get; set; }
        public int OrderId { get; set; }
        public bool IsReviewed { get; set; }

    }
}
