using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementApp.DTOs.Event;
using EventManagementApp.Models;

public interface ITicketService
{
    Task<PaymentResponseDTO> BookTicket(TicketDTO ticketDto, int UserID);
    Task<Tickets> ConfirmTicket(ConfirmPaymentDTO confirmPaymentDTO);
    Task<Tickets> CheckInTicket(string ticketId, int numberOfTicketsToCheckIn);
    Task<List<Tickets>> GetTicketsForUser(int userId);
}
