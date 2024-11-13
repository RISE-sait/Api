namespace Api.Model.People.Employees
{
    public class Admin(string name, string email, string phoneNumber, string bankAccountNumber) : Employee(name, email, phoneNumber, bankAccountNumber);
}