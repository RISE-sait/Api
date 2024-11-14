using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.People.Customers
{
    public class Customer : Account
    {
        public Customer(string name, string email, string phoneNumber, int credit = 0, int balance = 0) : base(name, email, phoneNumber)
        {
            Credit = credit;
            Balance = balance;
        }

        public Customer(string name, string email, Guid familyId, RolesEnum role, string? phoneNumber, int credit = 0, int balance = 0) : base(name, email)
        {

            if (role == RolesEnum.Parent && string.IsNullOrEmpty(phoneNumber))
            {
                throw new NotImplementedException();
            }

            if (!string.IsNullOrEmpty(phoneNumber)) {
                PhoneNumber = phoneNumber;
            }

            FamilyId = familyId;
            Role = role;
            Credit = credit;
            Balance = balance;
        }
        public bool HasConsentMarketingEmails { get; set; }
        public bool HasConsentMarketingSms { get; set; }
        public bool ShouldReceiveReceiptsForAllPayments { get; set; }
        public int Credit { get; set; }
        public int Balance { get; set; }
        public BasicAthleteInfo? BasicAthleteInfo { get; set; }
        public AdvancedAthleteInfo? AdvancedAthleteInfo { get; set; }

        [ForeignKey("Family")]
        public Guid? FamilyId { get; set; }
        public Family? Family { get; set; }
        public RolesEnum? Role { get; set; }
        public enum RolesEnum
        {
            Child,
            Parent
        }
    }
}
