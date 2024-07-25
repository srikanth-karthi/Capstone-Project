namespace EventManagementApp.DTOs.Event
{
    public class PaymentResponseDTO
    {
        public string RazorpayOrderId { get; set; }
        public string contentId { get; set; }
        public decimal Amount { get; set; }
    }
}
