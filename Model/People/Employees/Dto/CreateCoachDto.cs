using System.ComponentModel.DataAnnotations;

namespace Api.Model.People.Employees.Dto
{
    public record CreateCoachDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{10,}$", ErrorMessage = "Bank account number must be at least 10 digits.")]
        public string BankAccountNumber { get; set; } = null!;
    }
}