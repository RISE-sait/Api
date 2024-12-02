using System.ComponentModel.DataAnnotations;

namespace Api.Model.People
{
    public abstract class Account(string name, string email, string? phoneNumber = null)
    {
        [EmailAddress]
        public string Email { get; set; } = email;

        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; } = name;

        public string? PhoneNumber { get; set; } = phoneNumber;

        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string ProfilePic { get; set; } = string.Empty;
    }
}