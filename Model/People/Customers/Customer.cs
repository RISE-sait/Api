using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Api.Model.People.Customers.Customer;

namespace Api.Model.People.Customers
{
    [NotMapped]
    public sealed class Customer(Guid id, string name, string email, string? phoneNumber = null, int credit = 0, int balance = 0, Guid? familyId = null, RolesEnum? role = null)
    : Account(name, email, phoneNumber), IValidatableObject
    {
        public Guid Id { get; init; } = id;
        public bool HasConsentMarketingEmails { get; set; }
        public bool HasConsentMarketingSms { get; set; }
        public bool ShouldReceiveReceiptsForAllPayments { get; set; }
        public int Credit { get; set; } = credit;
        public int Balance { get; set; } = balance;
        public BasicAthleteInfo? BasicAthleteInfo { get; set; }
        public AdvancedAthleteInfo? AdvancedAthleteInfo { get; set; }
        public Guid? FamilyId { get; set; } = familyId;

        [NotMapped]
        public Family? Family { get; set; }
        public RolesEnum? Role { get; set; } = role;
        public enum RolesEnum
        {
            Child,
            Parent
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (FamilyId == null != (Role == null))
            {
                results.Add(new ValidationResult(
                    "Both FamilyId and Role must be defined together, or both should be null.",
                    [nameof(FamilyId), nameof(Role)]));
            }


            if (Role == RolesEnum.Parent && string.IsNullOrEmpty(PhoneNumber))
            {
                results.Add(new ValidationResult("Phone number is required for Parent role.", [nameof(PhoneNumber)]));
            }

            return results;
        }
    }
}
