using EventManagementApp.DTOs;
using EventManagementApp.DTOs.Event;
using EventManagementApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagementApp.Interfaces.Service
{
    public interface IEventService
    {
        Task<List<EventDTO>> GetAllEvents();
        Task<List<EventDTO>> GetEventsForUser();
        Task<List<TicketDTO>> GetTicketsForEvent(int eventId);
        Task<Event> UpdateEvent(UpdateEventDto eventDto, int eventId);
        Task<Event> AddEvent(AddEventDto eventDto);
    }
}
