using System.Threading.Tasks;

namespace EventManagementApp.Interfaces.Service
{
    public interface IPaymentService
    {
        Task<string> CreateOrder(decimal amount);
        Task<bool> VerifyPayment(string paymentId, string orderId, string signature);
    }
}
