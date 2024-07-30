using EventManagementApp.Context;
using EventManagementApp.DTOs.ScheduledEvent;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Repositories
{
    public class ScheduledEventRepository : Repository<ScheduledEvent, int>, IScheduledEventRepository
    {
        public ScheduledEventRepository(EventManagementDBContext _context) : base(_context)
        {
        }

        public async Task<List<AdminScheduledEventListDTO>> GetScheduledEvents()
        {
            var scheduledEvents = await _context.ScheduledEvents
                .Select(e => new AdminScheduledEventListDTO
                {
                    ScheduledEventId = e.ScheduledEventId,
                    ClientName = e.User.FullName,
                     ClientEmail = e.User.Email,
                    EventCategory = e.EventCategory.EventName,
                    CateringInstructions = e.QuotationRequest.CateringInstructions,
                    FoodPreference = (Enums.FoodPreference)e.QuotationRequest.FoodPreference,
                    LocationDetails = e.QuotationRequest.LocationDetails,
                    RequestDate = e.QuotationRequest.RequestDate,
                    SpecialInstructions = e.QuotationRequest.SpecialInstructions,
                    EventStartDate = e.QuotationRequest.EventStartDate,
                    EventEndDate = e.QuotationRequest.EventEndDate,
                    VenueType = e.QuotationRequest.VenueType,
                    IsCompleted = e.IsCompleted

                })
                .OrderBy(e => e.EventStartDate)
                .ToListAsync();
            return scheduledEvents;
        }

        public async Task<ScheduledEvent> GetScheduledEventByClietResponseId(int clientResponseId)
        {
            ScheduledEvent? scheduledEvent = await _context.ScheduledEvents.FirstOrDefaultAsync(s => s.ClienResponseId == clientResponseId);
            return scheduledEvent;
        }

        public async Task<ScheduledEvent> GetScheduledEventByUserId(int eventId, int userId)
        {
            ScheduledEvent? scheduledEvent = await _context.ScheduledEvents.FirstOrDefaultAsync(s => s.UserId == userId && s.ScheduledEventId == eventId);
            return scheduledEvent;
        }

        public async Task<List<BasicScheduledEventListDTO>> GetUserScheduledEvents(int userId)
        {
            var scheduledEvents = await _context.ScheduledEvents
                    .Where(e => e.UserId == userId)
                    .Select(e => new BasicScheduledEventListDTO
                    {
                        ScheduledEventId = e.ScheduledEventId,
                        EventCategory = e.EventCategory.EventName,
                        CateringInstructions = e.QuotationRequest.CateringInstructions,
                        FoodPreference = e.QuotationRequest.FoodPreference,
                        LocationDetails = e.QuotationRequest.LocationDetails,
                        RequestDate = e.QuotationRequest.RequestDate,
                        SpecialInstructions = e.QuotationRequest.SpecialInstructions,
                        EventStartDate = e.QuotationRequest.EventStartDate,
                        EventEndDate = e.QuotationRequest.EventEndDate,
                        VenueType = e.QuotationRequest.VenueType,
                        IsCompleted = e.IsCompleted,
                        OrderId = e.ClientResponse.Order.OrderId,
                        IsReviewed = e.ClientResponse.Review == null ? false : true

                    })
                    .OrderBy(e => e.EventStartDate)
                    .ToListAsync();
            return scheduledEvents;
        }
    }
}
