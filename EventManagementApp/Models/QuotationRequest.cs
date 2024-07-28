using System.ComponentModel.DataAnnotations;
using EventManagementApp.Enums;

namespace EventManagementApp.Models
{
    public class QuotationRequest
    {
        [Key]
        public int QuotationRequestId { get; set; }
        public int UserId { get; set; } 
        public int EventCategoryId { get; set; } 
        public VenueType VenueType { get; set; }
        public string LocationDetails { get; set; }
        public FoodPreference? FoodPreference { get; set; }
        public string? CateringInstructions { get; set; }
        public string SpecialInstructions { get; set; }
        public QuotationStatus QuotationStatus { get; set; } = QuotationStatus.Initiated;
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public DateTime RequestDate {get; set;} = DateTime.Now;
        public User User { get; set; }
        public EventCategory EventCategory { get; set; }
        public QuotationResponse QuotationResponse { get; set; }
    }
}