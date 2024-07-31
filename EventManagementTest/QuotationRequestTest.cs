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
    class QuotationRequestTest
    {
        private TestDBContext _context;
        private IQuotationRequestService _quotationRequestService;

        [SetUp]
        public void Setup()
        {
            _context = new TestDBContext(TestDBContext.GetInMemoryOptions());

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            IQuotationRequestRepository quotationRequestRepository = new QuotationRequestRepository(_context);
            IEventCategoryRepository _eventCategoryRepo = new EventCategoryRepository(_context);
            INotificationRepository notificationRepository = new NotificationRepository(_context);
            INotificationService notificationService = new NotificationService(notificationRepository);
            IUserRepository userRepository = new UserRespository(_context);
            _quotationRequestService = new QuotationRequestService(quotationRequestRepository, _eventCategoryRepo, notificationService, userRepository, _context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateQuotationRequest()
        {
            var quotationRequestDTO = new CreateQuotationRequestDTO
            {
                EventCategoryId = 1,
                VenueType = VenueType.PrivateVenue,
                LocationDetails = "This is location details",
                FoodPreference = FoodPreference.NoFood,
                CateringInstructions = null,
                SpecialInstructions = "No special instructions",
                EventStartDate = DateTime.Now.AddDays(1),
                EventEndDate = DateTime.Now.AddDays(2)
            };

            int id = await _quotationRequestService.CreateQuotationRequest(2, quotationRequestDTO);

            Assert.IsNotNull(id);
        }

        [Test]
        public async Task CreateQuotationRequestFail1()
        {

            var quotationRequestDTO = new CreateQuotationRequestDTO
            {
                EventCategoryId = 2,
                VenueType = VenueType.PrivateVenue,
                LocationDetails = "This is location details",
                FoodPreference = FoodPreference.NoFood,
                CateringInstructions = null,
                SpecialInstructions = "No special instructions",
                EventStartDate = DateTime.Now.AddDays(1),
                EventEndDate = DateTime.Now.AddDays(2)
            };

            Assert.ThrowsAsync<EventInActiveException>(async () =>
            {
                int id = await _quotationRequestService.CreateQuotationRequest(2, quotationRequestDTO);
            });

        }

        [Test]
        public async Task CreateQuotationRequestFail2()
        {

            var quotationRequestDTO = new CreateQuotationRequestDTO
            {
                EventCategoryId = 9999,
                VenueType = VenueType.PrivateVenue,
                LocationDetails = "This is location details",
                FoodPreference = FoodPreference.NoFood,
                CateringInstructions = null,
                SpecialInstructions = "No special instructions",
                EventStartDate = DateTime.Now.AddDays(1),
                EventEndDate = DateTime.Now.AddDays(2)
            };

            Assert.ThrowsAsync<NoEventCategoryFoundException>(async () => {
                int id = await _quotationRequestService.CreateQuotationRequest(2, quotationRequestDTO);
            });

        }
    }
}