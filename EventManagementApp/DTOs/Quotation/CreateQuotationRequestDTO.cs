using EventManagementApp.Enums;
using EventManagementApp.Validators;

namespace EventManagementApp.DTOs.QuotationRequest
{
    public class CreateQuotationRequestDTO
    {
        public int EventCategoryId { get; set; }
        public VenueType VenueType { get; set; }
        public string LocationDetails { get; set; }
        public FoodPreference FoodPreference { get; set; }
        public string? CateringInstructions { get; set; }
        public string SpecialInstructions { get; set; }
        
        [DateNotInPastAttribute]
        public DateTime EventStartDate { get; set; }

        [DateRange("EventStartDate")]
        public DateTime EventEndDate { get; set; }
    }
}
