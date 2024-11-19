using System.ComponentModel.DataAnnotations;

namespace Api.Model.People.Employees.Dto
{
    public readonly record struct CoachResponseDto(
         Guid Id,
         [MinLength(1)] string Name,
         [EmailAddress] string Email,
         [Phone] string PhoneNumber
     );

    public readonly record struct CreateCoachDto(
      [MinLength(1)] string Name,
      [EmailAddress] string Email,
      [Phone] string PhoneNumber,
      [RegularExpression(@"^\d{10,}$", ErrorMessage = "Bank account number must be at least 10 digits.")]
        string BankAccountNumber
  );

    public sealed record UpdateCoachDto(
        Guid Id,
       [MinLength(1)] string Name,
       [EmailAddress] string Email,
       [Phone] string PhoneNumber,
       [Url] string ProfilePic,
       [RegularExpression(@"^\d{10,}$", ErrorMessage = "Bank account number must be at least 10 digits.")]
        string BankAccountNumber
   );
}