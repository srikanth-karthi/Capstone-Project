using EventManagementApp.Enums;

namespace EventManagementApp.DTOs.QuotationRequest
{
    public class BasicQuotationRequestDTO
    {
        public int QuotationRequestId { get; set; }
        public string EventCategory { get; set; }
        public VenueType VenueType { get; set; }
        public string LocationDetails { get; set; }
        public FoodPreference FoodPreference { get; set; }
        public string? CateringInstructions { get; set; }
        public string SpecialInstructions { get; set; }
        public QuotationStatus QuotationStatus { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
