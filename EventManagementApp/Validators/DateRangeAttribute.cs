using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.Validators
{
    public class DateRangeAttribute : ValidationAttribute
    {
        public string StartDatePropertyName { get; }

        public DateRangeAttribute(string startDatePropertyName)
        {
            StartDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDateProperty = validationContext.ObjectType.GetProperty(StartDatePropertyName);
            if (startDateProperty == null)
            {
                return new ValidationResult($"Unknown property: {StartDatePropertyName}");
            }

            var startDateValue = (DateTime) startDateProperty.GetValue(validationContext.ObjectInstance);
            var endDateValue = (DateTime)value;

            if (endDateValue < startDateValue)
            {
                return new ValidationResult("EventEndDate should not be less than EventStartDate.");
            }

            return ValidationResult.Success;
        }
    }

}
