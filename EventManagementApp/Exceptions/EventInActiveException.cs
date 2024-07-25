namespace EventManagementApp.Exceptions
{
    public class EventInActiveException:Exception
    {
        public EventInActiveException(): base("Event is InActive. Can't create QuotationRequest") { }
    }
}
