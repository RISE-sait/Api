using System.ComponentModel.DataAnnotations;

namespace Api.Model.People.Customers.Dto
{
    public sealed record UpdateCustomerRequest(
        [MinLength(1)] string Name,
        [Phone] string PhoneNumber,
        bool HasConsentMarketingEmails,
        bool HasConsentMarketingSms,
        bool ShouldReceiveReceiptsForAllPayments,
        int Credit,
        int Balance,
        Guid? FamilyId = null,
        Customer.RolesEnum? Role = null
    ) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FamilyId == null != (Role == null))
            {
                yield return new ValidationResult(
                    "Both FamilyId and Role must be defined together, or both should be null.",
                    [nameof(FamilyId), nameof(Role)]
                );
            }
        }
    }
}