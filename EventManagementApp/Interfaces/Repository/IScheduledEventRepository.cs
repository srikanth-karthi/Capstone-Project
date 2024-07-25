using EventManagementApp.DTOs.ScheduledEvent;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Repository
{
    public interface IScheduledEventRepository:IRepository<ScheduledEvent, int>
    {
        public Task<List<AdminScheduledEventListDTO>> GetScheduledEvents();
        public Task<ScheduledEvent> GetScheduledEventByClietResponseId(int clientResponseId);
        public Task<ScheduledEvent> GetScheduledEventByUserId(int eventId, int userId);
        public Task<List<BasicScheduledEventListDTO>> GetUserScheduledEvents(int userId);
    }
}
