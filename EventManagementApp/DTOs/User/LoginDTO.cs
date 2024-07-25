using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.DTOs.User
{
    public class LoginDTO
    {
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is Invalid")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
