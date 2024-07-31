
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Repositories;
using EventManagementApp.Services;

namespace EventManagementTest
{
    class EventCategoryTest
    {
        private TestDBContext _context;
        private IEventCategoryService _eventService;

        [SetUp]
        public void Setup()
        {
            _context = new TestDBContext(TestDBContext.GetInMemoryOptions());

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            IEventCategoryRepository _eventCategoryRepo = new EventCategoryRepository(_context);
            _eventService = new EventCategoryService(_eventCategoryRepo);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllEventCategories()
        {

            var events = await _eventService.GetAllEventCategories();


            Assert.IsNotNull(events);
        }

    }
}