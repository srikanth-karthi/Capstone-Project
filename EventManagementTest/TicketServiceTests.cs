using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManagementApp.DTOs.Event;
using EventManagementApp.Enums;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using EventManagementApp.Repositories;
using EventManagementApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace EventManagementTest
{
    public class TicketServiceTests
    {
        private TestDBContext _context;
        private ITicketService _ticketService;
        private IPaymentService _paymentService;
        private IEventRepository _eventRepository;
        private ITicketRepository _ticketRepository;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<TestDBContext>();
            builder.UseInMemoryDatabase("TestDb")
                   .UseInternalServiceProvider(serviceProvider);

            _context = new TestDBContext(builder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _eventRepository = new EventRepository(_context);
            _ticketRepository = new TicketRepository(_context);
            _userRepository = new UserRespository(_context);
            _paymentService = new MockPaymentService();
            _ticketService = new TicketService(_userRepository, _ticketRepository, _eventRepository, _paymentService);

  
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


       

        [Test]
        public async Task BookTicket_ShouldCreateTicketAndReturnPaymentDetails()
        {
            // Arrange
            var ticketDto = new TicketDTO
            {
                EventId = 1,
                NumberOfTickets = 2
            };

            // Act
            var paymentResponse = await _ticketService.BookTicket(ticketDto, 1);

            // Assert
            Assert.IsNotNull(paymentResponse);
            Assert.IsNotNull(paymentResponse.RazorpayOrderId);
            Assert.AreEqual(260.0m, paymentResponse.Amount); 
            Assert.IsNotNull(paymentResponse.contentId);
        }

        [Test]
        public async Task ConfirmTicket_ShouldUpdatePaymentStatus()
        {
            // Arrange
            var ticketDto = new TicketDTO
            {
                EventId = 1,
                NumberOfTickets = 2
            };

            var paymentResponse = await _ticketService.BookTicket(ticketDto, 1);

            var confirmPaymentDTO = new ConfirmPaymentDTO
            {
                PaymentId = "fake_payment_id",
                RazorpayOrderId = paymentResponse.RazorpayOrderId,
                Signature = "fake_signature",
                ContentId = paymentResponse.contentId
            };

            // Act
            var ticket = await _ticketService.ConfirmTicket(confirmPaymentDTO);

            // Assert
            Assert.IsNotNull(ticket);
            Assert.AreEqual(PaymentStatus.Completed, ticket.PaymentStatus);
        }

        [Test]
        public async Task GetTicketsForUser_ShouldReturnUserTickets()
        {
            // Arrange
            var ticketDto = new TicketDTO
            {
                EventId = 1,
                NumberOfTickets = 2
            };

            await _ticketService.BookTicket(ticketDto, 1);
            await _ticketService.BookTicket(ticketDto, 1);

            // Act
            var tickets = await _ticketService.GetTicketsForUser(1);

            // Assert
            Assert.AreEqual(2, tickets.Count());
        }

        [Test]
        public async Task CheckInTicket_ShouldUpdateCheckedInTickets()
        {
            // Arrange
            var ticketDto = new TicketDTO
            {
                EventId = 1,
                NumberOfTickets = 2
            };

            var paymentResponse = await _ticketService.BookTicket(ticketDto, 1);
            var confirmPaymentDTO = new ConfirmPaymentDTO
            {
                PaymentId = "fake_payment_id",
                RazorpayOrderId = paymentResponse.RazorpayOrderId,
                Signature = "fake_signature",
                ContentId = paymentResponse.contentId
            };

            await _ticketService.ConfirmTicket(confirmPaymentDTO);

            // Act
            var checkedInTicket = await _ticketService.CheckInTicket(paymentResponse.contentId, 1);

            // Assert
            Assert.AreEqual(1, checkedInTicket.CheckedInTickets);
        }

        [Test]
        public async Task RepaymentTicket_ShouldReturnNewPaymentDetails()
        {
            // Arrange
            var ticketDto = new TicketDTO
            {
                EventId = 1,
                NumberOfTickets = 2
            };

            var paymentResponse = await _ticketService.BookTicket(ticketDto, 1);

            // Act
            var newPaymentResponse = await _ticketService.RepaymentTicket(paymentResponse.contentId);

            // Assert
            Assert.IsNotNull(newPaymentResponse);
            Assert.IsNotNull(newPaymentResponse.RazorpayOrderId);
            Assert.AreEqual(260.0m, newPaymentResponse.Amount); // 2 tickets * 20.0 cost per ticket
        }
    }

    public class MockPaymentService : IPaymentService
    {
        public Task<string> CreateOrder(decimal amount)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        public Task<bool> VerifyPayment(string paymentId, string orderId, string signature)
        {
            return Task.FromResult(true);
        }
    }


}
