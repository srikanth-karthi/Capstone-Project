using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventManagementApp.Services
{
    public class TokenService:ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            var value = configuration.GetSection("TokenKey").GetSection("JWT").Value;
            string _secretKey = value != null ? value.ToString() : throw new NullReferenceException("Cannot get JWT Secret key from Configuration file");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>(){
                new Claim("userId", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var myToken = new JwtSecurityToken(null, null, claims, expires: DateTime.Now.AddDays(2), signingCredentials: credentials);
            string token = new JwtSecurityTokenHandler().WriteToken(myToken);
            return token;
        }
    }
}
