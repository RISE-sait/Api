using System.ComponentModel.DataAnnotations;

namespace Api.Model.Facilities.Dto
{
    public sealed record CreateFacilityTypeRequest(
        [StringLength(30, MinimumLength = 1)] string Name
    );

    public sealed record UpdateFacilityTypeDto(
        Guid Id,
        [MaxLength(30)] string Name);
}