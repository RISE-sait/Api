using System.ComponentModel.DataAnnotations;

namespace Api.Model.Facilities.Dto
{
    public readonly record struct CreateFacilityDto(
        [param: Required] string Name,
        [param: Required] string Location,
        [param: Required] Guid FacilityTypeId
    );

    public readonly record struct PutFacilityDto(
        [param: Required] Guid FacilityId,
        [param: Required] string Name,
        [param: Required] string Location,
        [param: Required] Guid FacilityTypeId
    );

    public readonly record struct FacilityResponseDto(
        [param: Required] Guid Id,
        [param: Required] string Name,
        [param: Required] string Location,
        [param: Required] string FacilityTypeName
    );
}