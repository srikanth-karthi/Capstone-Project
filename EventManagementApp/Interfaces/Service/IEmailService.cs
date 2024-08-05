namespace EventManagementApp.Interfaces.Service
{
    public interface IEmailService
    {
        Task SendEmail(string receiverEmail, string subject, string message);
    }
}
