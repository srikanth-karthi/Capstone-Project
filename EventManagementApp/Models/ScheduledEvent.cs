using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.Models
{
    public class ScheduledEvent
    {
        [Key]
        public int ScheduledEventId { get; set; }
        public int EventCategoryId { get; set; } // Foreign Key
        public int ClienResponseId { get; set; } // Foreign Key
        public int QuotationRequestId { get; set; } // Foreign Key
        public bool IsCompleted { get; set; } = false;
        public int UserId { get; set; } // Foreign Key
        public EventCategory EventCategory { get; set; }
        public ClientResponse ClientResponse { get; set; }
        public User User { get; set; }
        public QuotationRequest QuotationRequest { get; set; }
    }
}
