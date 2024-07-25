using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.DTOs.User
{
    public class RegisterDTO : LoginDTO
    {

        [RegularExpression(@"^\d{10}$", ErrorMessage = "The phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
    }
}
