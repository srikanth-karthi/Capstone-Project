namespace EventManagementApp.Exceptions
{
    public class EventNotCompletedException: Exception
    {
        public EventNotCompletedException(): base("Event is not yet completed"){ }
    }
}
