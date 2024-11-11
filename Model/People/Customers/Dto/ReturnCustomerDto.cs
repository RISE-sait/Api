namespace Api.Model.People.Customers.Dto
{
    public record ReturnCustomerDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string>? FamilyMembers { get; set; }
    }    
}