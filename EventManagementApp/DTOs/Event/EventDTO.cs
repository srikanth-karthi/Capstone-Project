using EventManagementApp.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.DTOs
{
    public class EventDTO
    {
        public int? EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; }

  
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [EventDateValidation(ErrorMessage = "Event must be at least 2 days from now.")]
        public DateTime EventDate { get; set; }

        public bool IsActive { get; set; }=true;

        public string Poster { get; set; }

        public string Maplink { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Number of tickets must be greater than zero.")]
        public int NumberOfTickets { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ticket cost must be a positive value.")]
        public decimal TicketCost { get; set; }
        public int? RemainingTickets { get; set; }

    }
}
