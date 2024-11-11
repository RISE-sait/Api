using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.People.Customers
{
    public class Family
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }
        public List<Customer> Members { get; } = [];
    }
}