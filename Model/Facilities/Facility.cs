using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.Facilities
{
    public class Facility(string name, string location, Guid facilityTypeId)
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }

        public string Location { get; set; } = location;
        public string Name { get; set; } = name;
        
        [ForeignKey("FacilityType")]
        public Guid FacilityTypeId { get; set; } = facilityTypeId;
        public FacilityType FacilityType { get; set; } = null!;
    }

    public class FacilityType(string name)
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public string Name { get; set; } = name;
    }
}