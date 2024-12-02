namespace Api.Model.People.Customers;

public struct Family(Guid id, IEnumerable<Customer> members)
{
    public Guid Id { get; init; } = id;
    public IEnumerable<Customer> Members { get; set; } = members;
}