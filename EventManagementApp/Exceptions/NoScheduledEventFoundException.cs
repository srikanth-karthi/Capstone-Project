namespace EventManagementApp.Exceptions
{
    public class NoScheduledEventFoundException:Exception
    {
        public NoScheduledEventFoundException() : base("No Scheduled Event is found with the given Id") { }

    }
}
