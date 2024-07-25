using EventManagementApp.DTOs.Event;
using EventManagementApp.Enums;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using EventManagementApp.Repositories;
using System;
using System.Threading.Tasks;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IPaymentService _paymentService;
    private readonly IUserRepository _userRepository;

    public TicketService(IUserRepository userRepository, ITicketRepository ticketRepository, IEventRepository eventRepository, IPaymentService orderService)
    {
        _ticketRepository = ticketRepository;
        _eventRepository = eventRepository;
        _paymentService = orderService;
        _userRepository = userRepository;
    }

    public async Task<PaymentResponseDTO> BookTicket(TicketDTO ticketDto,int UserID)
    {
        if (ticketDto == null) throw new ArgumentNullException(nameof(ticketDto));

        var ticketId = Guid.NewGuid().ToString("N").Substring(0, 6);
        var eventDetails = await _eventRepository.GetById(ticketDto.EventId);

        if (eventDetails == null) throw new InvalidOperationException("Event not found.");

        var amount = ticketDto.NumberOfTickets * eventDetails.TicketCost;
        var orderId = await _paymentService.CreateOrder(amount);

        var user = await _userRepository.GetById(UserID);

        if (user == null) throw new InvalidOperationException("User not found.");

        var newTicket = new Tickets
        {
            EventId = ticketDto.EventId,
            UserId = UserID,
            AttendeeName = user.FullName,
            AttendeeEmail = user.Email,
            NumberOfTickets = ticketDto.NumberOfTickets,
            CheckedInTickets = ticketDto.NumberOfTickets,
            TicketCost = eventDetails.TicketCost,
            TicketId = ticketId,
            PaymentStatus = PaymentStatus.Pending 
        };

        await _ticketRepository.Add(newTicket);

        return new PaymentResponseDTO
        {
            RazorpayOrderId = orderId,
            contentId = ticketId,
            Amount = amount
        };
    }

    public async Task<Tickets> ConfirmTicket(ConfirmPaymentDTO confirmPaymentDTO)
    {


        var isPaymentValid = await _paymentService.VerifyPayment(confirmPaymentDTO.PaymentId, confirmPaymentDTO.RazorpayOrderId, confirmPaymentDTO.Signature);
        if (!isPaymentValid)
        {
            throw new InvalidOperationException("Payment verification failed.");
        }

        var ticket = await _ticketRepository.GetById(confirmPaymentDTO.ContentId);
        if (ticket == null)
        {
            throw new InvalidOperationException("Ticket not found.");
        }

        ticket.PaymentStatus = PaymentStatus.Completed;
        await _ticketRepository.Update(ticket);

        return ticket;
    }

    public async Task<List<Tickets>> GetTicketsForUser(int userId)
    {
        return await _ticketRepository.GetTicketsForUser(userId);
    }

    public async Task<Tickets> CheckInTicket(string ticketId, int numberOfTicketsToCheckIn)
    {
        var ticket = await _ticketRepository.CheckInTicket(ticketId, numberOfTicketsToCheckIn);
        if (ticket == null)
        {
            throw new InvalidOperationException("Ticket not found or invalid check-in attempt.");
        }
        return ticket;
    }
}