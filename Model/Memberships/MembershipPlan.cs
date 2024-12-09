using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Model.Memberships
{
    public class MembershipPlan(string name, long price, Guid membershipId)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string Name { get; set; } = name;
        public long Price { get; set; } = price;
        public RecurringPaymentPlan? RecurringPaymentPlan { get; set; }

        [ForeignKey("Membership")]
        public Guid MembershipId { get; set; } = membershipId;
        public Membership Membership { get; set; } = null!;
    }

    [Owned]
    public class RecurringPaymentPlan
    {
        public PaymentFrequency PaymentFrequency { get; set; } = PaymentFrequency.Weekly;
        public int? AmtPeriods { get; set; }
    }

    public enum PaymentFrequency
    {
        Weekly,
        BiWeekly,
        Monthly,
    }
}