namespace EventManagementApp.Exceptions
{
    public class ClientAlreadyRespondedException: Exception
    {
        public ClientAlreadyRespondedException() : base("Client is already responded to the Quotation") { }
    }
}
