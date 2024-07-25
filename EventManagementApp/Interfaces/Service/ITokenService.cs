using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Service
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
