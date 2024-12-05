using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.People.Staff
{
    public sealed class Staff(string name, string email, string phoneNumber, Guid staffTypeId): Account(name, email, phoneNumber) {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        
        [ForeignKey("StaffType")]
        public Guid StaffTypeId { get; set; } = staffTypeId;
        public StaffType StaffType { get; set; } = null!;
    }
}