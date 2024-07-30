using EventManagementApp.Validators;
using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.DTOs.Event
{
    public class UpdateEventDto
    {
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public IFormFile? Poster { get; set; }
        public string? Maplink { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number of tickets must be greater than zero.")]
        public int? AddedTicket { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ticket cost must be a positive value.")]
        public decimal? TicketCost { get; set; }
    }
}
