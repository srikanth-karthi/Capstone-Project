using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagementApp.Context;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using EventManagementApp.Services;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Repositories
{
    public class TicketRepository : Repository<Tickets, string>, ITicketRepository
    {
        public TicketRepository(EventManagementDBContext context) : base(context)
        {
        }

        public async Task<List<Tickets>> GetTicketsForEvent(int eventId)
        {
            return await _context.Tickets
                .Where(t => t.EventId == eventId)
                .ToListAsync();
        }

        public async Task<List<Tickets>> GetTicketsForUser(int userId)
        {
            return await _context.Tickets
                .Include(t => t.Event)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)  
                .ToListAsync();
        }

        public async Task<Tickets> CheckInTicket(string ticketId, int numberOfTicketsToCheckIn)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                return null;
            }
            if(ticket.PaymentStatus==PaymentStatus.Pending) {
                throw new InvalidOperationException("Payment verification failed.");
            }
            if (ticket.CheckedInTickets + numberOfTicketsToCheckIn <= ticket.NumberOfTickets)
            {
                ticket.CheckedInTickets += numberOfTicketsToCheckIn;
            }
            else
            {
                throw new NotEnoughTicketsException("Not enough tickets available.");
            }

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public override async Task<Tickets> Add(Tickets ticket)
        {
            var eventToUpdate = await _context.Events.FindAsync(ticket.EventId);
            if (eventToUpdate == null)
            {
                throw new EventNotFoundExceptions(ticket.EventId);
            }

            if (eventToUpdate.RemainingTickets < ticket.NumberOfTickets)
            {
                throw new NotEnoughTicketsException(eventToUpdate.RemainingTickets.ToString());
            }

            eventToUpdate.RemainingTickets -= ticket.NumberOfTickets;
            _context.Events.Update(eventToUpdate);

            await base.Add(ticket); 
            await _context.SaveChangesAsync();

            return ticket;
        }
    }
}
