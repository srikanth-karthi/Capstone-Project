using EventManagementApp.DTOs.Event;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Repository
{
    public interface IEventRepository : IRepository<Event, int>
    {

        Task<List<TicketDTO>> GetTicketsForEvent(int eventId);
    }
}
