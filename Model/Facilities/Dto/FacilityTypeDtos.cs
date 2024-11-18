using System.ComponentModel.DataAnnotations;

namespace Api.Model.Facilities.Dto
{
    public readonly record struct CreateFacilityTypeDto(
        [param: Required, StringLength(30, MinimumLength = 1)] string Name
    );

    public readonly record struct UpdateFacilityTypeDto(
            [param: Required] Guid Id,
            [param: Required, MaxLength(100)] string Name);
}