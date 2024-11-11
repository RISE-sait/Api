using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Model.People
{
    public abstract class Account(string name, string email)
    {
        public Account(string name, string email, string phoneNumber) : this(name, email) {
            PhoneNumber = phoneNumber;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }

        [EmailAddress]
        public string Email { get; set; } = email;

        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; } = name;

        public string? PhoneNumber { get; set; }

        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string ProfilePic { get; set; } = string.Empty;
    }
}