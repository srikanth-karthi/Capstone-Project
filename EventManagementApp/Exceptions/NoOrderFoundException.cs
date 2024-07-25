namespace EventManagementApp.Exceptions
{
    public class NoOrderFoundException: Exception
    {
        public NoOrderFoundException(): base("No order is found with the given Id") { }
    }
}
