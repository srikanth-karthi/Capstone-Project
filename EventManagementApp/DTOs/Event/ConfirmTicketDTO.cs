using System;

namespace EventManagementApp.DTOs.Event
{
    public class ConfirmPaymentDTO
    {
        public string ContentId { get; set; }
        public string PaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
        public string Signature { get; set; }
    }
}
