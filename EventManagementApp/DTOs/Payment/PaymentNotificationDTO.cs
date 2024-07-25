using EventManagementApp.Enums;

namespace EventManagementApp.DTOs.Payment
{
    public class PaymentNotificationDTO
    {
        public string TransactionId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }

    }
}
