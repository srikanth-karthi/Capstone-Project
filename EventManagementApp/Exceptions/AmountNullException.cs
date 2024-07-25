namespace EventManagementApp.Exceptions
{
    public class AmountNullException:Exception
    {
        public AmountNullException(): base("Amount should not be empty") { }
    }
}
