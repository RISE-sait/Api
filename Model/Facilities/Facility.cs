using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.Facilities
{
    public class Facility(string name, string location, Guid typeId)
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }

        public string Location { get; set; } = location;
        public string Name { get; set; } = name;

        [ForeignKey("Type")]
        public Guid FacilityTypeId { get; set; } = typeId;
        public FacilityType Type { get; set; } = null!;
    }

    public class FacilityType(string name)
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string Name { get; set; } = name;
    }
}