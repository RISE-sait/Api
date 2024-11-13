using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.Facilities
{
    public class Facility(string name, string location)
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id {get; init;}
        
        public string Location { get; set; } = location;
        public string Name { get; set; } = name;
    }
}