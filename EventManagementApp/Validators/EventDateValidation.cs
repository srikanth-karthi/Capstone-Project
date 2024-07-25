using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.Validators
{
    public class EventDateValidation: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime eventDate)
            {
                if (eventDate < DateTime.Now.AddDays(2))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
