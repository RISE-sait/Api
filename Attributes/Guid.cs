using System.ComponentModel.DataAnnotations;

namespace Api.Attributes
{

    [AttributeUsage(AttributeTargets.Parameter,
   AllowMultiple = false)]
    public class GuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is Guid guid)
            {
                return guid != Guid.Empty;
            }

            return false;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (!IsValid(value))
            {
                return new ValidationResult("The provided GUID is either invalid or empty.", [validationContext.MemberName ?? "Unknown member"]);
            }

            return ValidationResult.Success;
        }
    }
}