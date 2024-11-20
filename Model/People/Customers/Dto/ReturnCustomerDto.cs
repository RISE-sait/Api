namespace Api.Model.People.Customers.Dto
{
    public readonly record struct CustomerResponseDto
    (
        Guid Id,
        string Name,
        string Email,
        string? PhoneNumber = null,
        List<string>? FamilyMembers = null
    );
}