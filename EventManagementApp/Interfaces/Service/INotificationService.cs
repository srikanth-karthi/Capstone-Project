using EventManagementApp.DTOs.NotificationDTOs;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Service
{
    public interface INotificationService
    {

        public Task<List<NotificationReturnDTO>> GetAllNotifications(int userId);


    }
}
