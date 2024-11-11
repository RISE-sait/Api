using System.ComponentModel.DataAnnotations;
using Api.Model.People.Customers;

namespace Api.Model
{
    public class FinancialInfo(string bankAccountNumber)
    {
        [Key]
        public Guid Id {get; init;}
        public ICollection<Customer> Customers { get; set; } = [];

        public string BankAccountNumber { get; set; } = bankAccountNumber;
    }
}