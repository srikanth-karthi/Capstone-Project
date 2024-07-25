namespace EventManagementApp.DTOs.Payment
{
    public class PaymentResponseDTO
    {
        public string TransactionId { get; set; }
        public string PaymentURL { get; set; }
        public double PaymentExpiresEpoch { get; set; }

    }
}
