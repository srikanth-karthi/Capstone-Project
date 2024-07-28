﻿using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.DTOs.ReviewDTO;
using EventManagementApp.DTOs.ScheduledEvent;
using EventManagementApp.DTOs.User;
using EventManagementApp.Exceptions;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Service
{
    public interface IUserService
    {
        public Task<List<BasicQuotationRequestDTO>> GetUserRequests(int userId);

        public Task<UserQuotationRequestDTO> GetUserRequestById(int userId, int quotationRequestId);
        public Task<List<UserOrderListReturnDTO>> GetUserOrders(int userId);
        public Task<List<BasicScheduledEventListDTO>> GetUserEvents(int userId);

        public Task ReviewAnOrder(int userId, int orderId, ReviewDTO reviewDTO);
    }
}
