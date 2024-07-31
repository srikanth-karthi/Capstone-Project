
using System.Security.Cryptography;
using System.Text;
using EventManagementApp.Context;
using EventManagementApp.Enums;
using EventManagementApp.Models;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;

namespace EventManagementTest
{
    class TestDBContext : EventManagementDBContext
    {
        public TestDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public static DbContextOptions<TestDBContext> GetInMemoryOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDBContext>();
            optionsBuilder.UseInMemoryDatabase("EventManagementDB");
            return optionsBuilder.Options;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
     
            modelBuilder.Entity<User>().HasData(CreateUser());


            modelBuilder.Entity<EventCategory>()
                .HasData(
                    new EventCategory
                    {
                        EventCategoryId = 1,
                        IsService = true,
                        Poster="testposter",
                        EventName = "Testing Event",
                        Description = "Testing Description",
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    }
                );

            #region Inactive EventCategory
            modelBuilder.Entity<EventCategory>()
                .HasData(
                    new EventCategory
                    {
                        EventCategoryId = 2,
                        EventName = "Testing Event",
                        IsService = true,
                        Poster = "testposter",
                        Description = "Testing Description",
                        CreatedDate = DateTime.Now,
                        IsActive = false
                    }
                );
            #endregion

            modelBuilder.Entity<Event>()
                .HasData(
                    new Event
                    {
                        EventId = 1,
                        Poster = "testposter",
                        EventName = "Testing Event",
                        Description = "Testing Description",
                        EventDate = DateTime.Now,
                        IsActive = true,
                        TicketCost = 130,
                        NumberOfTickets=100,
                        RemainingTickets=50,
                        Maplink="testmaplink"


                    }
                ); ;

                        modelBuilder.Entity<Tickets>()
                .HasData(
                    new Tickets
                    {
                        EventId = 1,
                        TicketId="1234",
                           CheckedInTickets=25,
                           NumberOfTickets=50,
                           TicketCost=130,
                           AttendeeEmail="testemail",
                           AttendeeName="testusername",
                           CreatedAt = DateTime.Now,
                           PaymentStatus= PaymentStatus.Completed,
                           UserId=2
                    }
                );


            #region Data for Creating ClientResponse

            modelBuilder.Entity<QuotationRequest>()
                .HasData(
                    new QuotationRequest
                    {
                        QuotationRequestId = 1,
                        EventCategoryId = 1,
                        VenueType = VenueType.PrivateVenue,
                        LocationDetails = "Testing location",
                        FoodPreference = FoodPreference.NoFood,
                        SpecialInstructions = "Special Instructions",
                        EventStartDate = DateTime.Parse("2025-06-27T03:18:23.396Z"),
                        EventEndDate = DateTime.Parse("2025-06-28T03:18:23.396Z"),
                        UserId = 2,
                        RequestDate = DateTime.Now,
                        QuotationStatus = QuotationStatus.Responded
                    }
                );


            modelBuilder.Entity<QuotationResponse>()
                .HasData(
                    new QuotationResponse
                    {
                        QuotationResponseId = 1,
                        QuotationRequestId = 1,
                        RequestStatus = RequestStatus.Accepted,
                        QuotedAmount = 1000,
                        Currency = Currency.INR,
                        ResponseMessage = "sample message",
                        ResponseDate = DateTime.Now
                    }
                );

            #endregion

            #region Data that throw RequestNotApprovedException
            modelBuilder.Entity<QuotationRequest>()
                .HasData(
                    new QuotationRequest
                    {
                        QuotationRequestId = 2,
                        EventCategoryId = 1,
                        VenueType = VenueType.PrivateVenue,
                        LocationDetails = "Testing location",
                        FoodPreference = FoodPreference.NoFood,
                        SpecialInstructions = "Special Instructions",
                        EventStartDate = DateTime.Parse("2025-06-27T03:18:23.396Z"),
                        EventEndDate = DateTime.Parse("2025-06-28T03:18:23.396Z"),
                        UserId = 2,
                        RequestDate = DateTime.Now,
                        QuotationStatus = QuotationStatus.Responded
                    }
                );


            modelBuilder.Entity<QuotationResponse>()
                .HasData(
                    new QuotationResponse
                    {
                        QuotationResponseId = 2,
                        QuotationRequestId = 2,
                        RequestStatus = RequestStatus.Denied,
                        QuotedAmount = null,
                        ResponseMessage = "sample message",
                        ResponseDate = DateTime.Now
                    }
                );
            #endregion

            #region Data that throw ClientAlreadyRespondedException
            modelBuilder.Entity<QuotationRequest>()
                .HasData(
                    new QuotationRequest
                    {
                        QuotationRequestId = 3,
                        EventCategoryId = 1,
                        VenueType = VenueType.PrivateVenue,
                        LocationDetails = "Testing location",
                        FoodPreference = FoodPreference.NoFood,
                        SpecialInstructions = "Special Instructions",
                        EventStartDate = DateTime.Parse("2025-06-27T03:18:23.396Z"),
                        EventEndDate = DateTime.Parse("2025-06-28T03:18:23.396Z"),
                        UserId = 2,
                        RequestDate = DateTime.Now,
                        QuotationStatus = QuotationStatus.Responded
                    }
                );


            modelBuilder.Entity<QuotationResponse>()
                .HasData(
                    new QuotationResponse
                    {
                        QuotationResponseId = 3,
                        QuotationRequestId = 3,
                        RequestStatus = RequestStatus.Accepted,
                        QuotedAmount = null,
                        ResponseMessage = "sample message",
                        ResponseDate = DateTime.Now
                    }
                );



            modelBuilder.Entity<ClientResponse>()
                .HasData(
                        new ClientResponse
                        {
                            ClientDecision = ClientDecision.Accepted,
                            ClientResponseId = 1,
                            ClientResponseDate = DateTime.Now,
                            QuotationResponseId = 3
                        }
                );
            #endregion

            #region Data for Creating QuotationResponse
            modelBuilder.Entity<QuotationRequest>()
               .HasData(
                   new QuotationRequest
                   {
                       QuotationRequestId = 4,
                       EventCategoryId = 1,
                       VenueType = VenueType.PrivateVenue,
                       LocationDetails = "Testing location",
                       FoodPreference = FoodPreference.NoFood,
                       SpecialInstructions = "Special Instructions",
                       EventStartDate = DateTime.Parse("2025-06-27T03:18:23.396Z"),
                       EventEndDate = DateTime.Parse("2025-06-28T03:18:23.396Z"),
                       UserId = 2,
                       RequestDate = DateTime.Now,
                       QuotationStatus = QuotationStatus.Initiated
                   }
               );

            #endregion

            #region Data for marking ScheduledEvent completed

            modelBuilder.Entity<ScheduledEvent>()
                .HasData(
                    new ScheduledEvent
                    {
                        ScheduledEventId = 1,
                        ClienResponseId = 1,
                        EventCategoryId = 1,
                        IsCompleted = false,
                        QuotationRequestId = 1,
                        UserId = 2
                    }
                );
            #endregion

            #region Completed Order

            modelBuilder.Entity<Order>()
                .HasData(
                    new Order
                    {
                        OrderId = 1,
                        ClientResponseId = 1,
                        EventCategoryId = 1,
                        OrderDate = DateTime.Now,
                        UserId = 2,
                        TotalAmount = 1000,
                        Currency = Currency.INR,
                        OrderStatus = OrderStatus.Completed
                    }
                );

            #endregion

            #region Completed ScheduledEvent

            modelBuilder.Entity<QuotationRequest>()
                .HasData(
                    new QuotationRequest
                    {
                        QuotationRequestId = 5,
                        EventCategoryId = 1,
                        VenueType = VenueType.PrivateVenue,
                        LocationDetails = "Testing location",
                        FoodPreference = FoodPreference.NoFood,
                        SpecialInstructions = "Special Instructions",
                        EventStartDate = DateTime.Parse("2025-06-27T03:18:23.396Z"),
                        EventEndDate = DateTime.Parse("2025-06-28T03:18:23.396Z"),
                        UserId = 2,
                        RequestDate = DateTime.Now,
                        QuotationStatus = QuotationStatus.Responded
                    }
                );


            modelBuilder.Entity<QuotationResponse>()
                .HasData(
                    new QuotationResponse
                    {
                        QuotationResponseId = 4,
                        QuotationRequestId = 5,
                        RequestStatus = RequestStatus.Accepted,
                        QuotedAmount = null,
                        ResponseMessage = "sample message",
                        ResponseDate = DateTime.Now
                    }
                );



            modelBuilder.Entity<ClientResponse>()
                .HasData(
                        new ClientResponse
                        {
                            ClientDecision = ClientDecision.Accepted,
                            ClientResponseId = 2,
                            ClientResponseDate = DateTime.Now,
                            QuotationResponseId = 4
                        }
                );

            modelBuilder.Entity<Order>()
                .HasData(
                    new Order
                    {
                        OrderId = 2,
                        EventCategoryId = 1,
                        UserId = 2,
                        ClientResponseId = 2,
                        OrderStatus = OrderStatus.Completed,
                        OrderDate = DateTime.Now,
                        Currency = Currency.INR,
                        TotalAmount = 1000
                    }
                );


            modelBuilder.Entity<ScheduledEvent>()
                .HasData(
                    new ScheduledEvent
                    {
                        ScheduledEventId = 2,
                        EventCategoryId = 1,
                        UserId = 2,
                        ClienResponseId = 2,
                        IsCompleted = true,
                        QuotationRequestId = 5
                    }
                );


            #endregion

    
            modelBuilder.Entity<QuotationRequest>()
               .HasData(
                   new QuotationRequest
                   {
                       QuotationRequestId = 6,
                       EventCategoryId = 1,
                       VenueType = VenueType.PrivateVenue,
                       LocationDetails = "Testing location",
                       FoodPreference = FoodPreference.NoFood,
                       SpecialInstructions = "Special Instructions",
                       EventStartDate = DateTime.Parse("2025-06-27T03:18:23.396Z"),
                       EventEndDate = DateTime.Parse("2025-06-28T03:18:23.396Z"),
                       UserId = 2,
                       RequestDate = DateTime.Now,
                       QuotationStatus = QuotationStatus.Responded
                   }
               );


            modelBuilder.Entity<QuotationResponse>()
                .HasData(
                    new QuotationResponse
                    {
                        QuotationResponseId = 5,
                        QuotationRequestId = 6,
                        RequestStatus = RequestStatus.Accepted,
                        QuotedAmount = null,
                        ResponseMessage = "sample message",
                        ResponseDate = DateTime.Now
                    }
                );



            modelBuilder.Entity<ClientResponse>()
                .HasData(
                        new ClientResponse
                        {
                            ClientDecision = ClientDecision.Accepted,
                            ClientResponseId = 3,
                            ClientResponseDate = DateTime.Now,
                            QuotationResponseId = 5
                        }
                );

            modelBuilder.Entity<Order>()
                .HasData(
                    new Order
                    {
                        OrderId = 3,
                        EventCategoryId = 1,
                        UserId = 2,
                        ClientResponseId = 3,
                        OrderStatus = OrderStatus.Pending,
                        OrderDate = DateTime.Now,
                        TotalAmount = 1000,
                        Currency = Currency.INR
                    }
                );

    
        

            base.OnModelCreating(modelBuilder);
        }

        private User CreateAdminUser()
        {
            User admin = new User();

            admin.Role = UserType.Admin;

            admin.UserId = 1;
            admin.FullName = "Book My Event";
            admin.Email = "bookmyevent24@gmail.com";
            return admin;
        }
        private User CreateUser()
        {
            User admin = new User();

            admin.Role = UserType.Admin;

            admin.UserId = 2;
            admin.FullName = "testusername";
            admin.Email = "testuser@gmail.com";
            return admin;
        }
    }
}