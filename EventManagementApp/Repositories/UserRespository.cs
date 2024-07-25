using EventManagementApp.Context;
using EventManagementApp.DTOs.ClientResponse;
using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.DTOs.Quotation;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.DTOs.User;
using EventManagementApp.Enums;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Repositories
{
    public class UserRespository : Repository<User, int>, IUserRepository
    {
        public UserRespository(EventManagementDBContext context) : base(context) { }

        public async Task<User> GetUserByEmail(string email)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }


        public async Task<UserQuotationRequestDTO> GetUserRequestById(int userId, int quotationRequestId)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            UserQuotationRequestDTO? userQuotationRequestDTO = await _context.QuotationRequests
                .Where(q => q.UserId == userId && q.QuotationRequestId == quotationRequestId)
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
                                IsPaid = q.QuotationResponse.ClientResponse.Order.OrderStatus == OrderStatus.Completed ? true: false
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

        public async Task<List<BasicQuotationRequestDTO>> GetUserRequests(int userId)
        {
            List<BasicQuotationRequestDTO> quotationRequest = await _context
                .QuotationRequests
                .Where(q => q.UserId == userId)
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

            return quotationRequest;
        }

        public async Task<List<UserOrderListReturnDTO>> GetUserOrders(int userId)
        {
            List<UserOrderListReturnDTO> userOrders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new UserOrderListReturnDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    TotalAmount = o.TotalAmount,
                    Currency = o.Currency,
                    EventDetails = new UserQuotationRequestDTO
                    {
                        QuotationRequestId = o.ClientResponse.QuotationResponse.QuotationRequest.QuotationRequestId,
                        EventCategory = o.ClientResponse.QuotationResponse.QuotationRequest.EventCategory.EventName,
                        VenueType = o.ClientResponse.QuotationResponse.QuotationRequest.VenueType,
                        LocationDetails = o.ClientResponse.QuotationResponse.QuotationRequest.LocationDetails,
                        FoodPreference = o.ClientResponse.QuotationResponse.QuotationRequest.FoodPreference,
                        CateringInstructions = o.ClientResponse.QuotationResponse.QuotationRequest.CateringInstructions,
                        SpecialInstructions = o.ClientResponse.QuotationResponse.QuotationRequest.SpecialInstructions,
                        QuotationStatus = o.ClientResponse.QuotationResponse.QuotationRequest.QuotationStatus,
                        EventStartDate = o.ClientResponse.QuotationResponse.QuotationRequest.EventStartDate,
                        EventEndDate = o.ClientResponse.QuotationResponse.QuotationRequest.EventEndDate,
                        RequestDate = o.ClientResponse.QuotationResponse.QuotationRequest.RequestDate,
                        QuotationResponse = null
                    }
                })
                .OrderByDescending(q => q.OrderDate)
                .ToListAsync();
            return userOrders;
        }

        public async Task<Order> GetUserOrder(int UserId, int OrderId)
        {
            Order? order = await _context.Orders
                .Include(o=>o.ClientResponse)
                .ThenInclude(c=>c.Review)
                .FirstOrDefaultAsync(o=>o.OrderId == OrderId && o.UserId == UserId);
            return order;
        }



    }
}