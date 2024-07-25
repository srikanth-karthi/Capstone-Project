using EventManagementApp.Context;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Repositories
{
    public class OrderRepository : Repository<Order, int>, IOrderRepository
    {
        public OrderRepository(EventManagementDBContext _context) : base(_context)
        {
        }

        public async Task<Order> GetUserOrderById(int userId, int orderId)
        {
            Order? order = await _context.Orders
                .Include(o => o.ClientResponse)
                    .ThenInclude(cr => cr.QuotationResponse)
                        .ThenInclude(a=> a.QuotationRequest)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderId == orderId);

            return order;
        }
    }
}
