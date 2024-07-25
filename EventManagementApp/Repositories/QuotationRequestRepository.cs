using EventManagementApp.Context;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.Enums;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Repositories
{
    public class QuotationRequestRepository : Repository<QuotationRequest, int>, IQuotationRequestRepository
    {
        public QuotationRequestRepository(EventManagementDBContext _context) : base(_context)
        {

        }

        public async Task<List<BasicQuotationRequestDTO>> GetAllWithEventName(bool IsNew)
        {
            
            List<BasicQuotationRequestDTO> basicQuotationRequestDTOs = await _context.QuotationRequests
                .Where(q=> IsNew == true ? q.QuotationStatus == QuotationStatus.Initiated : true)
                .Select(q => new BasicQuotationRequestDTO
                {
                    QuotationRequestId = q.QuotationRequestId,
                    EventCategory = q.EventCategory.EventName,
                    VenueType = q.VenueType,
                    LocationDetails = q.LocationDetails,
                    FoodPreference = q.FoodPreference,
                    CateringInstructions = q.CateringInstructions,
                    SpecialInstructions = q.SpecialInstructions,
                    QuotationStatus = q.QuotationStatus,
                    EventStartDate = q.EventStartDate,
                    EventEndDate = q.EventEndDate,
                    RequestDate = q.RequestDate
                })
                .OrderByDescending(q => q.RequestDate)
                .ToListAsync();
            return basicQuotationRequestDTOs;
        }

        public async Task<QuotationRequest> GetById(int id, int userId)
        {
            return await _context.QuotationRequests.FirstOrDefaultAsync(q=>q.QuotationRequestId == id && q.UserId == userId);
        }

        
    }
}
