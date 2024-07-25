using System.Collections.Generic;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.DTOs.ReviewDTO;
using EventManagementApp.DTOs.ScheduledEvent;
using EventManagementApp.DTOs.User;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;

namespace EventManagementApp.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IScheduledEventRepository _scheduledEventRepository;

        public UserService(IUserRepository userRepository, IScheduledEventRepository scheduledEventRepository)
        {
            _userRepository = userRepository;
            _scheduledEventRepository = scheduledEventRepository;
        }

        public async Task<UserQuotationRequestDTO> GetUserRequestById(int userId, int quotationRequestId)
        {
            UserQuotationRequestDTO quotationRequest = await _userRepository.GetUserRequestById(userId, quotationRequestId);
            
            if (quotationRequest == null)
            {
                throw new NoQuotationRequestFoundException();
            }
            return quotationRequest;
        }

        public async Task<List<BasicQuotationRequestDTO>> GetUserRequests(int userId)
        {
            List<BasicQuotationRequestDTO> quotationRequests = await _userRepository.GetUserRequests(userId);
            return quotationRequests;

        }

        public async Task<List<UserOrderListReturnDTO>> GetUserOrders(int userId)
        {
            return await _userRepository.GetUserOrders(userId);
        }

        public async Task ReviewAnOrder(int userId, int orderId, ReviewDTO reviewDTO)
        {
            Order userOrder = await _userRepository.GetUserOrder(userId, orderId);

            if (userOrder == null)
            {
                throw new NoOrderFoundException();
            }

            if (userOrder.ClientResponse.Review != null)
            {
                throw new OrderReviewedAlreadyException();
            }

            ScheduledEvent scheduledEvent = await _scheduledEventRepository.GetScheduledEventByClietResponseId(userOrder.ClientResponseId);

            if (scheduledEvent == null)
            {
                throw new PaymentNotCompletedException();
            }

            if (!scheduledEvent.IsCompleted)
            {
                throw new EventNotCompletedException();
            }

            User user = await _userRepository.GetById(userId);

            Review review = new Review
            {
                Rating = reviewDTO.Rating,
                Comments = reviewDTO.Comments,
                UserId = userId,
                ClientResponseId = userOrder.ClientResponseId,
                EventCategoryId = userOrder.EventCategoryId
            };

            user.Reviews = new List<Review> { review };
            await _userRepository.Update(user);

        }

        public async Task<List<BasicScheduledEventListDTO>> GetUserEvents(int userId)
        {
            List < BasicScheduledEventListDTO> scheduledEvents = await _scheduledEventRepository.GetUserScheduledEvents(userId);
            return scheduledEvents;
        }
    }
}
