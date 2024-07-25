using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.Validators
{
    public class DateNotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateValue = (DateTime)value;
            if (dateValue < DateTime.Now.Date)
            {
                return new ValidationResult("EventStartDate should not be in past");
            }
            return ValidationResult.Success;
        }
    }
}
