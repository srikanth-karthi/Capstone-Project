namespace EventManagementApp.Exceptions
{
    public class CurrencyNullException:Exception
    {
        public CurrencyNullException(): base("Currency should not be null") { }
    }
}
