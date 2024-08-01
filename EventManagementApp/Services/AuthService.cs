using System.Security.Cryptography;
using System.Text;
using Google.Apis.Auth;
using EventManagementApp.DTOs.User;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using static EventManagementApp.Controllers.AuthController;

namespace EventManagementApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly string _googleClientId;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _googleClientId = configuration["GoogleAuthSettings:ClientId"];
        }



        public async Task<LoginReturnDTO> AuthenticateGoogleUser(GoogleLoginRequest googleToken)
        {
            var payload = await ValidateGoogleToken(googleToken.Token);
            await Console.Out.WriteLineAsync(payload.ToString());
            User existingUser = await _userRepository.GetUserByEmail(payload.Email);
            if (existingUser != null)
            {
                return new LoginReturnDTO
                {
                    UserId = existingUser.UserId,
                    token = _tokenService.GenerateToken(existingUser)
                };
            }

            User user = new User
            {
                Email = payload.Email,
                FullName = payload.Name,
                ProfileUrl = payload.Picture,
                Role =UserType.User
            };
            user = await _userRepository.Add(user);

            return new LoginReturnDTO
            {
                UserId = user.UserId,
                token = _tokenService.GenerateToken(user)
            };
        }

        private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string googleToken)
        {

                return await GoogleJsonWebSignature.ValidateAsync(googleToken, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleClientId }
                });

        }


    }
}
