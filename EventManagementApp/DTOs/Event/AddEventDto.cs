using EventManagementApp.Validators;
using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.DTOs.Event
{
    public class AddEventDto
    {

        [Required]
        [StringLength(100)]
        public string EventName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [EventDateValidation(ErrorMessage = "Event must be at least 2 days from now.")]

        public DateTime EventDate { get; set; }

        [Required]
        public IFormFile Poster { get; set; }

        [Required]
        public string Maplink { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Number of tickets must be greater than zero.")]
        public int NumberOfTickets { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ticket cost must be a positive value.")]
        public decimal TicketCost { get; set; }

    }
}
