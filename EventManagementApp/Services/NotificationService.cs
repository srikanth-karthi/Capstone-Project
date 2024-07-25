using EventManagementApp.DTOs.NotificationDTOs;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using EventManagementApp.Repositories;

namespace EventManagementApp.Services
{
    public class NotificationService : INotificationService
    {
        private INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }




        public async Task<List<NotificationReturnDTO>> GetAllNotifications(int userId)
        {
            var notifications = await _notificationRepository.PendingOrders(userId);
            return notifications;
        }

    }
}
