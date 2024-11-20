using Api.Model.Facilities;
using Api.Model.Facilities.Dto;

namespace Api.Mappers
{
    /// <summary>
    /// Provides extension methods for mapping DTOs to Facility entities.
    /// </summary>
    public static class FacilityMapper
    {
        /// <summary>
        /// Maps a <see cref="CreateFacilityRequest"/> to a <see cref="Facility"/> entity.
        /// </summary>
        /// <param name="createFacilityDto">The DTO containing the facility creation data.</param>
        /// <returns>A <see cref="Facility"/> entity populated with the data from the DTO.</returns>
        public static Facility MapToFacility(this CreateFacilityRequest request)
        {
            return new Facility(
                request.Name,
                request.Location,
                request.FacilityTypeId
            );
        }
        
        public static FacilityResponseDto MapToFacilityResponseDto(this Facility facility)
        {
            return new FacilityResponseDto(
                facility.Id,
                facility.Name,
                facility.Location,
                facility.FacilityType.Name
            );
        }
    }
}