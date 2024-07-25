namespace EventManagementApp.Exceptions
{
    public class NoTransactionFoundException : Exception
    {
        public NoTransactionFoundException(): base("Transaction is not found for given Id") { }
    }
}
