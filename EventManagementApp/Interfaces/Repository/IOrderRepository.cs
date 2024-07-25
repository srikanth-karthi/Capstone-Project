using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Repository
{
    public interface IOrderRepository : IRepository<Order, int>
    {
        public Task<Order> GetUserOrderById(int userId, int orderId);
    }
}
