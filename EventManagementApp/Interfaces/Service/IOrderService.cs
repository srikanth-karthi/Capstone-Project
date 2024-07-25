using EventManagementApp.DTOs.Event;
using EventManagementApp.Models;
using System.Threading.Tasks;

namespace EventManagementApp.Interfaces.Service
{
    public interface IOrderService
    {
        Task<PaymentResponseDTO> PayForOrder(int orderId, int userId);
        Task<bool> ConfirmPayment(ConfirmPaymentDTO confirmPaymentDTO, int userId);
    }
}
