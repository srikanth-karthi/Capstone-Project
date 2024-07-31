
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Repositories;
using EventManagementApp.Services;

namespace EventManagementTest
{
    class ScheduledEventTest
    {
        private TestDBContext _context;
        private IScheduledEventService _scheduledEventService;

        [SetUp]
        public void Setup()
        {

            _context = new TestDBContext(TestDBContext.GetInMemoryOptions());
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            IScheduledEventRepository scheduledEventRepository = new ScheduledEventRepository(_context);

            _scheduledEventService = new SchedulesEventService(scheduledEventRepository);

        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task MarkAsCompletedTest()
        {

            await _scheduledEventService.MarkEventAsCompleted(1);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            await _scheduledEventService.MarkEventAsCompleted(1, 2);

        }


    }
}