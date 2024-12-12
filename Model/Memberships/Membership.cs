using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.Memberships
{
    public class Membership(string name, DateOnly startDateTime, DateOnly endDateTime, string? description)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }

        [Length(1, 50)]
        public string Name { get; set; } = name;        
        public string? Description { get; set; } = description;
        public DateOnly StartDateTime { get; set; } = startDateTime;
        public DateOnly EndDateTime { get; set; } = endDateTime;

        public ICollection<MembershipPlan> MembershipPlans { get; set; } = [];
    }
}