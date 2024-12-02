using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model
{
    public sealed class StaffType(string name, string description)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
    }
}