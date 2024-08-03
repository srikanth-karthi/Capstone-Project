using EventManagementApp.Context;
using EventManagementApp.DTOs.ClientResponse;
using EventManagementApp.DTOs.Quotation;
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
                    RequestDate = q.RequestDate,
                    Name=q.User.FullName,
                    Email=q.User.Email,

                })
                .OrderByDescending(q => q.RequestDate)
                .ToListAsync();
            return basicQuotationRequestDTOs;
        }


        public async Task<UserQuotationRequestDTO> GetRequestById(int quotationRequestId)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            UserQuotationRequestDTO? userQuotationRequestDTO = await _context.QuotationRequests
                .Where(q => q.QuotationRequestId == quotationRequestId)
                .Select(q => new UserQuotationRequestDTO
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
                    RequestDate = q.RequestDate,
                    QuotationResponse = q.QuotationResponse != null ? new UserQuotationResponseDTO
                    {
                        QuotationResponseId = q.QuotationResponse.QuotationResponseId,
                        RequestStatus = q.QuotationResponse.RequestStatus,
                        QuotedAmount = q.QuotationResponse.QuotedAmount,
                        Currency = q.QuotationResponse.Currency,
                        ResponseMessage = q.QuotationResponse.ResponseMessage,
                        ResponseDate = q.QuotationResponse.ResponseDate,
                        ClientResponse = q.QuotationResponse.ClientResponse != null ?
                            q.QuotationResponse.ClientResponse.ClientDecision == ClientDecision.Accepted ?
                            new ClientResponseDecisionDTO
                            {
                                ClientDecision = q.QuotationResponse.ClientResponse.ClientDecision,
                                OrderId = q.QuotationResponse.ClientResponse.Order.OrderId,
                                IsPaid = q.QuotationResponse.ClientResponse.Order.OrderStatus == OrderStatus.Completed ? true : false
                            } :
                            new ClientResponseDecisionDTO
                            {
                                ClientDecision = q.QuotationResponse.ClientResponse.ClientDecision,
                                OrderId = null,
                                IsPaid = null
                            }
                        : null,
                    } : null
                })
                .FirstOrDefaultAsync();
#pragma warning restore CS8601 // Possible null reference assignment.
            return userQuotationRequestDTO;
        }

        public async Task<QuotationRequest> GetById(int id, int userId)
        {
            return await _context.QuotationRequests.FirstOrDefaultAsync(q=>q.QuotationRequestId == id && q.UserId == userId);
        }

        
    }
}
