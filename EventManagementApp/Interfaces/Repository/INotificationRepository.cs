using EventManagementApp.DTOs.NotificationDTOs;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Repository
{
    public interface INotificationRepository
    {

        public Task<List<NotificationReturnDTO>> PendingOrders(int userId);
    }
}
