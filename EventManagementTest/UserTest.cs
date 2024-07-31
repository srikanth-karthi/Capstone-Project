
using EventManagementApp.DTOs.ReviewDTO;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Repositories;
using EventManagementApp.Services;

namespace EventManagementTest
{
    class UserTest
    {
        private TestDBContext _context;
        private IUserService _userService;
        private INotificationService _notificationService;

        [SetUp]
        public void Setup()
        {

            _context = new TestDBContext(TestDBContext.GetInMemoryOptions());
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            IQuotationResponseRepository _quotationRequestRepo = new QuotationResponseRepository(_context);

            IUserRepository userRepo = new UserRespository(_context);
            IScheduledEventRepository scheduledEventRepository = new ScheduledEventRepository(_context);
            INotificationRepository notificationRepository = new NotificationRepository(_context);
            _notificationService = new NotificationService(notificationRepository);
            _userService = new UserService(userRepo, scheduledEventRepository);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetUserRequests()
        {
            var requests = await _userService.GetUserRequests(2);
            Assert.IsNotNull(requests);
        }

        [Test]
        public async Task GetUserRequestById()
        {
            var request = await _userService.GetUserRequestById(2, 6);
            Assert.IsNotNull(request);
        }

        [Test]
        public async Task GetUserRequestByIdFail1()
        {

            Assert.ThrowsAsync<NoQuotationRequestFoundException>(async () =>
            {
                var request = await _userService.GetUserRequestById(2, 99999);
            });

        }


        [Test]
        public async Task GetUserOrders()
        {
            var orders = await _userService.GetUserOrders(2);
            Assert.IsNotNull(orders);
        }



        [Test]
        public async Task GetUserEvents()
        {
            var events = await _userService.GetUserEvents(2);
            Assert.IsNotNull(events);
        }

        [Test]
        public async Task ReviewAOrder()
        {
            ReviewDTO dto = new ReviewDTO
            {
                Comments = "This is comment",
                Rating = 4
            };

            await _userService.ReviewAnOrder(2, 2, dto);
        }

        [Test]
        public async Task ReviewAOrderFail1()
        {
            ReviewDTO dto = new ReviewDTO
            {
                Comments = "This is comment",
                Rating = 4
            };

            Assert.ThrowsAsync<NoOrderFoundException>(async () =>
            {
                await _userService.ReviewAnOrder(2, 9999, dto);
            });

        }

        [Test]
        public async Task ReviewAOrderFail2()
        {
            ReviewDTO dto = new ReviewDTO
            {
                Comments = "This is comment",
                Rating = 4
            };

            await _userService.ReviewAnOrder(2, 2, dto);

            Assert.ThrowsAsync<OrderReviewedAlreadyException>(async () =>
            {
                await _userService.ReviewAnOrder(2, 2, dto);

            });

        }


        [Test]
        public async Task ReviewAOrderFail3()
        {
            ReviewDTO dto = new ReviewDTO
            {
                Comments = "This is comment",
                Rating = 4
            };


            Assert.ThrowsAsync<EventNotCompletedException>(async () =>
            {
                await _userService.ReviewAnOrder(2, 1, dto);
            });

        }



        [Test]
        public async Task GetNotifications()
        {
            var notifications = await _notificationService.GetAllNotifications(2);
            Assert.IsNotNull(notifications);
        }


    }

}