namespace EventManagementApp.Exceptions
{
    public class RequestNotApprovedException:Exception
    {
        public RequestNotApprovedException(): base("Request is not approved by admin. Can't create client response") { }
    }
}
