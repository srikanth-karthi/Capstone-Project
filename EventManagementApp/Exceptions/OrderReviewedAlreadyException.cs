namespace EventManagementApp.Exceptions
{
    public class OrderReviewedAlreadyException: Exception
    {
        public OrderReviewedAlreadyException(): base("Given Order is already Reviewed") { }
    }
}
