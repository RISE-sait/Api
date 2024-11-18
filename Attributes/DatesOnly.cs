using System.ComponentModel.DataAnnotations;

namespace Api.Attributes
{

    [AttributeUsage(AttributeTargets.Parameter,
   AllowMultiple = false)]
    public class ValidDateOnlyAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validates the specified value to ensure it is a valid DateOnly object and not the default value.
        /// </summary>
        /// <param name="value">The value to validate, expected to be a DateOnly object.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> indicating whether the value is valid or not.
        /// Returns a success result if the value is a valid DateOnly object and not the default value.
        /// Returns a failure result with an error message if the value is the default DateOnly value.
        /// </returns>
        /// 
        public override bool IsValid(object? value)
        {
            if (value is DateOnly date)
            {
                return date != default;
            }

            return false;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (!IsValid(value))
            {
                return new ValidationResult("The date must be a valid, non-default DateOnly value.");
            }

            return ValidationResult.Success;
        }
    }
}