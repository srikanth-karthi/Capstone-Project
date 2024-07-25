using System.Collections.Generic;
using System.Threading.Tasks;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Repository
{
    public interface ITicketRepository : IRepository<Tickets, string>
    {
        Task<List<Tickets>> GetTicketsForEvent(int eventId);
        Task<List<Tickets>> GetTicketsForUser(int userId);
        Task<Tickets> CheckInTicket(string ticketId, int numberOfTicketsToCheckIn);
    }
}
