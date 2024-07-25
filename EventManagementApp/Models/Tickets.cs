using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EventManagementApp.Enums;

namespace EventManagementApp.Models
{
    public class Tickets
    {
        [Key]
        public string TicketId { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public  User User { get; set; }

        [Required]
        public string AttendeeName { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.Now;

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        public string AttendeeEmail { get; set; }

        public int CheckedInTickets { get; set; }

        public int NumberOfTickets { get; set; }

        public decimal TicketCost { get; set; }

        public  Event Event { get; set; }
    }
}
