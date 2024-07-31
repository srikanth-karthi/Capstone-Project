
using EventManagementApp.Context;
using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Repositories;
using EventManagementApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace EventManagementTest
{
    class AdminTest
    {
        private TestDBContext _context;
        private IAdminService _adminService;

        [SetUp]
        public void Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("EventManagementDB");
            _context = new TestDBContext(optionsBuilder.Options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();


            IConfiguration configuration = new InMemoryConfiguration().Configuration;


            IEventCategoryRepository _eventCategoryRepo = new EventCategoryRepository(_context);
            IScheduledEventRepository _scheduledRepo = new ScheduledEventRepository(_context);
            IQuotationRequestRepository _quotationRequestRepo = new QuotationRequestRepository(_context);
            IBlobService _blobService = new BlobService(configuration);

            _adminService = new AdminService(_eventCategoryRepo, _scheduledRepo, _quotationRequestRepo, _blobService);



        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateEventCategoryTest()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var content = "Fake file content";
            var fileName = "test.png";
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.Write(content);
            writer.Flush();
            memoryStream.Position = 0;

            mockFile.Setup(_ => _.OpenReadStream()).Returns(memoryStream);
            mockFile.Setup(_ => _.FileName).Returns(fileName);
            mockFile.Setup(_ => _.Length).Returns(memoryStream.Length);

            CreateEventCategoryDTO createEventCategoryDTO = new CreateEventCategoryDTO
            {
                EventName = "Testing Event",
                Description = "Testing Description",
                IsService = true,
                Poster = mockFile.Object
            };

            // Act
            await _adminService.CreateEventCategory(createEventCategoryDTO);

            // Assert
            var eventCategory = await _context.EventCategories.FirstOrDefaultAsync(e => e.EventName == createEventCategoryDTO.EventName);
            Assert.IsNotNull(eventCategory);
            Assert.AreEqual(createEventCategoryDTO.EventName, eventCategory.EventName);
            Assert.AreEqual(createEventCategoryDTO.Description, eventCategory.Description);
        }
    

    [Test]
        public async Task GetScheduledEvents()
        {
            // Act
            var events = await _adminService.GetScheduledEvents();

            // Assert
            Assert.IsNotNull(events);
        }

        [Test]
        public async Task UpdateEventDetails()
        {
 

            var updateEventCategoryDTO = new UpdateEventCategoryDTO
            {
                EventName = "Updated Event Name",
                Description = "Updated Description",
                IsActive = true
            };

            // Act
     
            await _adminService.UpdateEventDetails(1, updateEventCategoryDTO);

            // Assert
            var eventCategory = await _context.EventCategories.FirstOrDefaultAsync(e => e.EventName == updateEventCategoryDTO.EventName);
            Assert.IsNotNull(eventCategory);

        }

        [Test]
        public void UpdateEventDetailsFail1()
        {
            // Arrange

            var updateEventCategoryDTO = new UpdateEventCategoryDTO
            {
                EventName = "Updated Event Name",
                Description = "Updated Description",
                IsActive = true
            };

            Assert.ThrowsAsync<NoEventCategoryFoundException>(async () =>
            {
                await _adminService.UpdateEventDetails(999, updateEventCategoryDTO);
            });
        }

        [Test]
        public async Task UpdateEventDetailsFail2()
        {




            var updateEventCategoryDTO = new UpdateEventCategoryDTO();


            Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _adminService.UpdateEventDetails(1, updateEventCategoryDTO);
            });
        }

        [Test]
        public async Task GetAllEventCategories()
        {
            var eventCategories = await _adminService.GetAllEventCategories();
            Assert.IsNotNull(eventCategories);
        }

        [Test]
        public async Task GetQuotations()
        {
            // Act
            var events = await _adminService.GetQuotations(isNew: true);

            // Assert
            Assert.IsNotNull(events);
        }
    }
}