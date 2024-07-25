using EventManagementApp.Context;
using EventManagementApp.DTOs.NotificationDTOs;
using EventManagementApp.Enums;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementApp.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly EventManagementDBContext _context;

        public NotificationRepository(EventManagementDBContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationReturnDTO>> PendingOrders(int userId)
        {
            var pendingOrders = await _context.Orders
               .Where(o => o.UserId == userId && o.OrderStatus == OrderStatus.Pending)
               .Include(e => e.EventCategory)
               .ToListAsync();

            return pendingOrders.Select(a => ToDto(a)).ToList();
        }

        private NotificationReturnDTO ToDto(Order order)
        {
            return new NotificationReturnDTO
            {
                OrderId = order.OrderId,
                Message = order.EventCategory.EventName
            };
        }
    }
}
