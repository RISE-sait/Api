using System.ComponentModel.DataAnnotations;

namespace Api.Model.Facilities.Dto
{
    public sealed record CreateFacilityTypeDto(
        [StringLength(30, MinimumLength = 1)] string Name
    );

    public sealed record UpdateFacilityTypeDto(
        [param: Required] Guid Id,
        [param: Required, MaxLength(30)] string Name);
}