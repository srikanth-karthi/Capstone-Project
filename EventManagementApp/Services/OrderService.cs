using EventManagementApp.DTOs.Event;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using EventManagementApp.Repositories;
using System;
using System.Threading.Tasks;

namespace EventManagementApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentService _paymentService;
        private readonly IScheduledEventRepository _scheduledEventRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IScheduledEventRepository ScheduledEventRepository,IUserRepository userRepository, ITicketRepository ticketRepository, IOrderRepository orderRepository, IPaymentService paymentService)
        {
            _ticketRepository = ticketRepository;
            _orderRepository = orderRepository;
            _paymentService = paymentService;
            _scheduledEventRepository = ScheduledEventRepository;
            _userRepository = userRepository;
        }

        public async Task<PaymentResponseDTO> PayForOrder(int Orderid, int userId)
        {
            var OrderDetails = await _orderRepository.GetUserOrderById(userId,Orderid) ?? throw new InvalidOperationException("Order not found.");
            if (OrderDetails.OrderStatus == OrderStatus.Completed)
                throw new PaymentAlreadyCompletedException();

            var orderId = await _paymentService.CreateOrder((decimal)OrderDetails.TotalAmount);

            return new PaymentResponseDTO
            {
                contentId = OrderDetails.OrderId.ToString(),
                Amount = (decimal)OrderDetails.TotalAmount,
                RazorpayOrderId = orderId
            };
        }

        public async Task<bool> ConfirmPayment(ConfirmPaymentDTO confirmPaymentDTO ,int UserId)
        {
            var isPaymentValid = await _paymentService.VerifyPayment(confirmPaymentDTO.PaymentId, confirmPaymentDTO.RazorpayOrderId, confirmPaymentDTO.Signature);
            if (!isPaymentValid)
            {
                throw new InvalidOperationException("Payment verification failed.");
            }

            var order = await _orderRepository.GetUserOrderById(UserId, int.Parse(confirmPaymentDTO.ContentId)) ?? throw new InvalidOperationException("Order not found.");
          
            
         

            if (order.OrderStatus == OrderStatus.Pending)
            {
                ScheduledEvent scheduledEvent = new()
                {
                    UserId = UserId,
                    ClienResponseId = order.ClientResponseId,
                    EventCategoryId = order.EventCategoryId,
                    QuotationRequestId = order.ClientResponse.QuotationResponse.QuotationRequestId
                };
                order.OrderStatus = OrderStatus.Completed;
                await _scheduledEventRepository.Add(scheduledEvent);
                await _orderRepository.Update(order);
            }
            return true;


        }


    }

    
}
