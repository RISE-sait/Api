using System.ComponentModel.DataAnnotations;

namespace Api.Model.People.Customers.Dto
{
    public record PatchCustomerDto : IValidatableObject
    {
        public string? Name { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        public bool? HasConsentMarketingEmails { get; set; }
        public bool? HasConsentMarketingSms { get; set; }
        public bool? ShouldReceiveReceiptsForAllPayments { get; set; }
        public int? Credit { get; set; }
        public int? Balance { get; set; }
        public Guid? FamilyId { get; set; }
        public Customer.RolesEnum? Role { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FamilyId == null != (Role == null))
            {
                yield return new ValidationResult(
                    "Both FamilyId and Role must be defined together, or both should be null.",
                    [nameof(FamilyId), nameof(Role)]);
            }
        }
    }
}
