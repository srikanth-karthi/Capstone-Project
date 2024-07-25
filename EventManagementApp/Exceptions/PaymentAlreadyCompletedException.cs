namespace EventManagementApp.Exceptions
{
    public class PaymentAlreadyCompletedException:Exception
    {
        public PaymentAlreadyCompletedException() : base("Payment is already Completed"){ }
    }
}
