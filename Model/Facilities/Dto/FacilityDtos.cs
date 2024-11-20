using System.ComponentModel.DataAnnotations;

namespace Api.Model.Facilities.Dto
{
    public sealed record CreateFacilityRequest(
        [StringLength(30, MinimumLength = 1)] string Name,
        [StringLength(30, MinimumLength = 1)] string Location,
        Guid FacilityTypeId
    );

    public sealed record PutFacilityRequest(
        Guid FacilityId,
        [StringLength(30, MinimumLength = 1)] string Name,
        [StringLength(30, MinimumLength = 1)] string Location,
        Guid FacilityTypeId
    );

    public readonly record struct FacilityResponseDto(
        Guid Id,
        string Name,
        string Location,
        string FacilityTypeName
    );
}