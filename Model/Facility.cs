using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model
{
    public class Facility(string location, string name)
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id {get; init;}
        
        public string Location { get; set; } = location;
        public string Name { get; set; } = name;
    }
}