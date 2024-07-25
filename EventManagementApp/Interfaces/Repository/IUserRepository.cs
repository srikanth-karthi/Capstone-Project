using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.DTOs.User;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User, int>
    {
        public Task<User> GetUserByEmail(string email);

        public Task<List<BasicQuotationRequestDTO>> GetUserRequests(int userId);
        public Task<UserQuotationRequestDTO> GetUserRequestById(int userId, int quotationRequestId);
        public Task<List<UserOrderListReturnDTO>> GetUserOrders(int userId);
        public Task<Order> GetUserOrder(int UserId, int OrderId);


    }
}
