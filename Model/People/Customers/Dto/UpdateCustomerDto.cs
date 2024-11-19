using System.ComponentModel.DataAnnotations;

namespace Api.Model.People.Customers.Dto
{
    public sealed record UpdateCustomerDto(
        string Name,
        string Email,
        string PhoneNumber,
        bool HasConsentMarketingEmails,
        bool HasConsentMarketingSms,
        bool ShouldReceiveReceiptsForAllPayments,
        int Credit,
        int Balance,
        Guid? FamilyId = null,
        Customer.RolesEnum? Role = null) : IValidatableObject
    {
        public string Name { get; set; } = Name;
        
        [EmailAddress]
        public string Email { get; set; } = Email;
        
        [Phone]
        public string PhoneNumber { get; set; } = PhoneNumber;
        
        public bool HasConsentMarketingEmails { get; set; } = HasConsentMarketingEmails;
        public bool HasConsentMarketingSms { get; set; } = HasConsentMarketingSms;
        public bool ShouldReceiveReceiptsForAllPayments { get; set; } = ShouldReceiveReceiptsForAllPayments;
        public int Credit { get; set; } = Credit;
        public int Balance { get; set; } = Balance;
        public Guid? FamilyId { get; set; } = FamilyId;
        public Customer.RolesEnum? Role { get; set; } = Role;

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
