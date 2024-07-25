using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;

namespace EventManagementApp.Services
{
    public class SchedulesEventService : IScheduledEventService
    {
        private readonly IScheduledEventRepository _scheduledEventRepository;

        public SchedulesEventService(IScheduledEventRepository scheduledEventRepository) {
            _scheduledEventRepository = scheduledEventRepository;
        }


        

        public async Task MarkEventAsCompleted(int eventId, int userId) 
        {
            ScheduledEvent? scheduledEvent = await _scheduledEventRepository.GetScheduledEventByUserId(eventId, userId);

            if (scheduledEvent == null)
            {
                throw new NoScheduledEventFoundException();
            }

            await MarkEventAsCompleted(scheduledEvent);
        }

        public async Task MarkEventAsCompleted(int eventId) 
        {
            ScheduledEvent? scheduledEvent = await _scheduledEventRepository.GetById(eventId);

            if (scheduledEvent == null)
            {
                throw new NoScheduledEventFoundException();
            }

            await MarkEventAsCompleted(scheduledEvent);
        }

        private async Task MarkEventAsCompleted(ScheduledEvent scheduledEvent)
        {
            scheduledEvent.IsCompleted = true;
            await _scheduledEventRepository.Update(scheduledEvent);
        }
    }
}
