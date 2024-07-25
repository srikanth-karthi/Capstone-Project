namespace EventManagementApp.Exceptions
{
    public class NoNotificationFoundException: Exception
    {
        public NoNotificationFoundException(): base("Notification is not found for given Id") { }
    }
}
