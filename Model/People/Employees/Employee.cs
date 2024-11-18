namespace Api.Model.People.Employees
{
    public class Employee(string name, string email, string phoneNumber): Account(name, email, phoneNumber);
}