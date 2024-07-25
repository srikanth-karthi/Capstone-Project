namespace EventManagementApp.Exceptions
{
    public class PaymentNotCompletedException:Exception
    {
        public PaymentNotCompletedException(): base("Payment is not completed. Complete it to proceed") { }
    }
}
