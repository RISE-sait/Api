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
        /// Maps a <see cref="CreateFacilityDto"/> to a <see cref="Facility"/> entity.
        /// </summary>
        /// <param name="createFacilityDto">The DTO containing the facility creation data.</param>
        /// <returns>A <see cref="Facility"/> entity populated with the data from the DTO.</returns>
        public static Facility MapToFacility(this CreateFacilityDto createFacilityDto)
        {
            return new Facility(
                createFacilityDto.Name,
                createFacilityDto.Location,
                createFacilityDto.FacilityTypeId
            );
        }
    }
}