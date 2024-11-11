using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Model.People.Customers;

namespace Api.Model.Courses
{
    public class Course(string name, DateOnly startDateTime, DateOnly endDateTime)
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id {get; init; }
        
        [Length(1, 50)]
        public string Name {get; set; } = name;
        public string Description {get; set; } = string.Empty;
        public DateOnly StartDateTime {get; set; } = startDateTime;
        public DateOnly EndDateTime {get; set; } = endDateTime;
        
        public Collection<Customer> Customers {get; set; } = [];
        
        public void DepositCredit() {

        }

        public void SignWaiver() {
            
        }
    }
}