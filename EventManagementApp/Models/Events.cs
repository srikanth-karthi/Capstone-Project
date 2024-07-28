using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; }


        public string Description { get; set; }

        public DateTime EventDate { get; set; }

        public bool IsActive { get; set; }

        public string Poster { get; set; }

        public string Maplink { get; set; }


        public int NumberOfTickets { get; set; }

        public int RemainingTickets { get; set; }

        public decimal TicketCost { get; set; }


        public virtual ICollection<Tickets> Tickets { get; set; }
    }


}
