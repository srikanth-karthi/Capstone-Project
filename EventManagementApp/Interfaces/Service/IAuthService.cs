using EventManagementApp.DTOs.User;
using static EventManagementApp.Controllers.AuthController;

namespace EventManagementApp.Interfaces.Service
{
    public interface IAuthService
    {


        Task<LoginReturnDTO> AuthenticateGoogleUser(GoogleLoginRequest googleToken);
    }
}
