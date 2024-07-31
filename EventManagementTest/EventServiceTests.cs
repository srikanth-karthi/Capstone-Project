using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EventManagementApp.DTOs;
using EventManagementApp.DTOs.Event;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using EventManagementApp.Repositories;
using EventManagementApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using static NUnit.Framework.Constraints.Tolerance;

namespace EventManagementTest
{
    public class EventServiceTests
    {
        private TestDBContext _context;
        private IEventService _eventService;
        private IBlobService _blobService;

        [SetUp]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<TestDBContext>();
            builder.UseInMemoryDatabase("TestDb")
                   .UseInternalServiceProvider(serviceProvider);
            IConfiguration configuration = new InMemoryConfiguration().Configuration;
            _context = new TestDBContext(builder.Options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var eventRepository = new EventRepository(_context);
            _blobService = new BlobService(configuration);
            _eventService = new EventService(eventRepository, _blobService);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task AddEvent_ShouldAddAndRetrieveEvent()
        {

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

  
            var addEventDto = new AddEventDto
            {
                EventName = "New Event",
                Description = "Event Description",
                EventDate = DateTime.UtcNow.AddDays(5),
                Maplink = "https://maps.google.com",
                Poster = mockFile.Object,
                NumberOfTickets = 100,
                TicketCost = 20.0m,
                
            };

            // Act
            var addedEvent = await _eventService.AddEvent(addEventDto);

            // Assert
            Assert.IsNotNull(addedEvent);
            Assert.AreEqual(addEventDto.EventName, addedEvent.EventName);
            Assert.AreEqual(addEventDto.Description, addedEvent.Description);
            Assert.AreEqual(addEventDto.EventDate, addedEvent.EventDate);
            Assert.AreEqual(addEventDto.Maplink, addedEvent.Maplink);

            Assert.AreEqual(addEventDto.NumberOfTickets, addedEvent.NumberOfTickets);
            Assert.AreEqual(addEventDto.TicketCost, addedEvent.TicketCost);


        }
  

        [Test]
        public async Task UpdateEvent_ShouldUpdateEventDetails()
        {


            var updateEventDto = new UpdateEventDto
            {
                Description = "Updated Description",
                Maplink = "https://updatedmaps.google.com",
   
                AddedTicket = 50,
                TicketCost = 25.0m,
                IsActive = false
            };

            // Act
            var updatedEvent = await _eventService.UpdateEvent(updateEventDto, 1);

            // Assert
            Assert.IsNotNull(updatedEvent);
            Assert.AreEqual(updateEventDto.Description, updatedEvent.Description);
            Assert.AreEqual(updateEventDto.Maplink, updatedEvent.Maplink);

            Assert.AreEqual(150, updatedEvent.NumberOfTickets);  // 100 + 50
            Assert.AreEqual(100, updatedEvent.RemainingTickets);  // 100 + 50
            Assert.AreEqual(updateEventDto.TicketCost, updatedEvent.TicketCost);
            Assert.AreEqual(updateEventDto.IsActive, updatedEvent.IsActive);
        }

        [Test]
        public async Task GetAllEvents_ShouldReturnAllEvents()
        {

            // Act
            var events = await _eventService.GetAllEvents();

            // Assert
            Assert.AreEqual(1, events.Count);
        }

        [Test]
        public async Task GetEventsForUser_ShouldReturnOnlyActiveEvents()
        {
           
            // Act
            var events = await _eventService.GetEventsForUser();

            // Assert
            Assert.AreEqual(1, events.Count);

        }

        [Test]
        public async Task GetTicketsForEvent_ShouldReturnTicketsForGivenEventId()
        {



            // Act
            var retrievedTickets = await _eventService.GetTicketsForEvent(1);

            // Assert
            Assert.AreEqual(1, retrievedTickets.Count);
        }
    }
}
