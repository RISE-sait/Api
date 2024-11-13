namespace Api.Model.People.Employees
{
    public class Employee(string name, string email, string phoneNumber, string bankAccountNumber): Account(name, email, phoneNumber)
    {
        public string BankAccountNumber { get; set; } = bankAccountNumber;
    }
}