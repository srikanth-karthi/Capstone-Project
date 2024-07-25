using EventManagementApp.Context;
using EventManagementApp.DTOs;
using EventManagementApp.DTOs.Event;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementApp.Repositories
{
    public class EventRepository : Repository<Event, int>, IEventRepository
    {
        public EventRepository(EventManagementDBContext _context) : base(_context)
        {
        }



        public async Task<List<TicketDTO>> GetTicketsForEvent(int eventId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.EventId == eventId)
                .Select(t => new TicketDTO
                {
                    TicketId = t.TicketId,
                    AttendeeName = t.AttendeeName,
                    AttendeeEmail = t.AttendeeEmail,
                    CheckedInTickets = t.CheckedInTickets,
                    NumberOfTickets = t.NumberOfTickets,
                    TicketCost = t.TicketCost
                })
                .ToListAsync();

            return tickets;
        }
    }
}
