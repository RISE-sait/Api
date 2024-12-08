// using System.ComponentModel.DataAnnotations.Schema;
//
// namespace Api.Model.Memberships
// {
//     public class MembershipPlan(string name, long price, Guid membershipId)
//     {
//         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//         public Guid Id { get; init; }
//         public string Name { get; set; } = name;
//         public long Price { get; set; } = price;
//         public RecurringPaymentPlan? RecurringPaymentPlan { get; set; }
//
//         [ForeignKey("Membership")]
//         public Guid MembershipId { get; set; } = membershipId;
//         public Membership Membership { get; set; } = null!;
//     }
//
//     public struct RecurringPaymentPlan
//     {
//         public long Price { get; set; }
//         public Period Period { get; set; }
//         public int? AmtPeriods { get; set; }
//     }
//
//     public enum Period
//     {
//         Weekly,
//         BiWeekly,
//         Monthly,
//     }
// }