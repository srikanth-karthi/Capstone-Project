using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int ClientResponseId { get; set; } // Foreign key
        public int EventCategoryId { get; set; } // Foreign Key
        public int UserId { get; set; } // Foreign Key
        public float Rating { get; set; }
        public string Comments { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public ClientResponse ClientResponse { get; set; }
        public EventCategory EventCategory { get; set; }
        public User User { get; set; }
    }
}
