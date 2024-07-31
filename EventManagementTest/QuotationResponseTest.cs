using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Repositories;
using EventManagementApp.Services;
using Microsoft.Extensions.Configuration;

namespace EventManagementTest
{
    class QuotationResponseTest
    {
        private TestDBContext _context;
        private IQuotationResponseService _quotationResponseService;

        [SetUp]
        public void Setup()
        {
            _context = new TestDBContext(TestDBContext.GetInMemoryOptions());

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            IQuotationRequestRepository quotationRequestRepository = new QuotationRequestRepository(_context);
            INotificationRepository notificationRepository = new NotificationRepository(_context);
            INotificationService notificationService = new NotificationService(notificationRepository);

            _quotationResponseService = new QuotationResponseService(quotationRequestRepository, notificationService, _context);

        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        [Test]
        public async Task CreateQuotationResponse()
        {
            var responseDTO = new CreateQuotationResponseDTO
            {
                QuotationRequestId = 4,
                RequestStatus = RequestStatus.Accepted,
                QuotedAmount = 25000,
                Currency = Currency.INR,
                ResponseMessage = "Response Message"
            };

            int id = await _quotationResponseService.CreateQuotationResponse(responseDTO);

            Assert.IsNotNull(id);
        }

        [Test]
        public void CreateQuotationResponseFail1()
        {
            var responseDTO = new CreateQuotationResponseDTO
            {
                QuotationRequestId = 1,
                RequestStatus = RequestStatus.Accepted,
                QuotedAmount = 25000,
                Currency = Currency.INR,
                ResponseMessage = "Response Message"
            };


            Assert.ThrowsAsync<QuotationAlreadyRespondedException>(async () =>
            {
                int id = await _quotationResponseService.CreateQuotationResponse(responseDTO);
            });

        }

        [Test]
        public async Task CreateQuotationResponseFail2()
        {
            var responseDTO = new CreateQuotationResponseDTO
            {
                QuotationRequestId = 4,
                RequestStatus = RequestStatus.Accepted,
                QuotedAmount = null,
                Currency = null,
                ResponseMessage = "Response Message"
            };


            Assert.ThrowsAsync<AmountNullException>(async () =>
            {
                int id = await _quotationResponseService.CreateQuotationResponse(responseDTO);
            });
        }
    }
}