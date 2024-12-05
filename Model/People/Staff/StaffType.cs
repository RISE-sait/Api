using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Model.Permissions;

namespace Api.Model.People.Staff
{
    public sealed class StaffType(string name)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        [Length(1, 20)] public string Name { get; set; } = name;
        
        public ICollection<StaffPermissions> Permissions { get; set; } = [];
        public ICollection<Staff> Staffs { get; set; } = [];
    }
}