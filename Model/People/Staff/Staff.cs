using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.People.Staff
{
    public sealed class Staff(string name, string email, string phoneNumber, Guid staffTypeId): Account(name, email, phoneNumber) {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public Guid StaffTypeId { get; set; } = staffTypeId;

        [ForeignKey("StaffTypeId")]
        public StaffType StaffType { get; set; } = null!;
    }
}