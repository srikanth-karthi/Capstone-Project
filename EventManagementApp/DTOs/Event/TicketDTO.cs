namespace EventManagementApp.DTOs.Event
{
    public class TicketDTO
    {
        public string? TicketId { get; set; }
        public string? AttendeeName { get; set; }
        public string? AttendeeEmail { get; set; }
        public int? CheckedInTickets { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal? TicketCost { get; set; }
        public int? UserId { get; set; }
        public int EventId { get; set; }

         public string? EventName { get; set; }
    }
}
