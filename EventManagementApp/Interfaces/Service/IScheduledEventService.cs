
using EventManagementApp.Enums;

namespace EventManagementApp.Interfaces.Service
{
    public interface IScheduledEventService
    {
        /// <exception cref="NoScheduledEventFoundException"/>
        public Task MarkEventAsCompleted(int eventId, int userId);

        /// <exception cref="NoScheduledEventFoundException"/>
        public Task MarkEventAsCompleted(int eventId);
    }
}
