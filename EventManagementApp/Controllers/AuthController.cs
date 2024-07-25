using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using EventManagementApp.DTOs.User;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Controllers
{
    [Route("api/")]
    [ApiController]
    [AllowAnonymous]
    [ExcludeFromCodeCoverage]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        public class GoogleLoginRequest
        {
            public string Token { get; set; }
        }
        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest  request)
        {
            try
            {
                var payload = await _authService.AuthenticateGoogleUser(request);
        

        
                return Ok(new { payload });
            }
            catch (InvalidJwtException)
            {
                return BadRequest("Invalid token.");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    


    

        [Route("verify")]
        [HttpGet]
        public async Task<IActionResult> VerifyUser()
        {
            try
            {

                var verifyReturn = new
                {
                    role = User.FindFirst(ClaimTypes.Role).Value.ToString() == "Admin" ? UserType.Admin : UserType.User
                };

                return Ok(verifyReturn);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


    }
}
