using System.ComponentModel.DataAnnotations;

namespace Api.Model.People.Staff
{
  public readonly record struct StaffResponseDto(
       Guid Id,
       [MinLength(1)] string Name,
       [EmailAddress] string Email,
       [Phone] string PhoneNumber
   );

  public sealed record CreateStaffRequest(
    [MinLength(1)] string Name,
    [EmailAddress] string Email,
    [Phone] string PhoneNumber,
      Guid StaffTypeId
);

  public sealed record UpdateStaffRequest(
      Guid Id,
     [MinLength(1)] string Name,
     [EmailAddress] string Email,
     [Phone] string PhoneNumber,
     [Url] string ProfilePic,
           Guid StaffTypeId

 );
}