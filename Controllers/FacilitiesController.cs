using Api.Attributes;
using Api.Database;
using Api.Mappers;
using Api.Model.Facilities;
using Api.Model.Facilities.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffTypeEnum = Api.Enums.StaffType;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [AuthorizeRoles([StaffTypeEnum.Admin])]
    public class FacilitiesController(AppDbContext context) : ControllerBase
    {

        /// <summary>
        /// Gets all facilities.
        /// </summary>
        /// <returns>A list of all facilities.</returns>
        /// <response code="200">Returns a list of all facilities.</response>
        [HttpGet]
        // [AuthorizeRoles([StaffTypeEnum.Coach])]
        [ProducesResponseType(typeof(IEnumerable<Facility>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFacilitiesAsync(int limit = 5)
        {
            var facilities = await context.Facilities.Include(f => f.FacilityType).Take(limit).ToListAsync();

            var facilityResponses = facilities.Select(f => f.MapToFacilityResponseDto()).ToList();

            return Ok(facilityResponses);
        }

        /// <summary>
        /// Gets a facility by its ID.
        /// </summary>
        /// <param name="id">The ID of the facility to retrieve.</param>
        /// <returns>The facility with the specified ID.</returns>
        /// <response code="200">Returns the facility with the specified ID.</response>
        /// <response code="404">If the facility is not found.</response>
        [HttpGet("{id:guid}")]
        [AuthorizeRoles([StaffTypeEnum.Coach])]
        [ProducesResponseType(typeof(FacilityResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFacilityById(Guid id)
        {
            var facility = await context.Facilities.Where(f => f.Id == id).Include(f => f.FacilityType).FirstOrDefaultAsync();
            if (facility == null)
                return NotFound();

            return Ok(facility.MapToFacilityResponseDto());
        }

        /// <summary>
        /// Creates a new facility.
        /// </summary>
        /// <param name="createFacilityDto">The DTO containing the facility creation data.</param>
        /// <returns>The created facility.</returns>
        /// <response code="201">Returns the newly created facility.</response>
        /// <response code="400">If the model state is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FacilityResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFacility([FromBody] CreateFacilityRequest createFacilityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var facility = createFacilityDto.MapToFacility();

            await context.Facilities.AddAsync(facility);
            await context.SaveChangesAsync();

            await context.Entry(facility).Reference(f => f.FacilityType).LoadAsync();

            return CreatedAtAction(nameof(GetFacilityById), new { id = facility.Id }, facility.MapToFacilityResponseDto());
        }

        /// <summary>
        /// Updates an existing facility.
        /// </summary>
        /// <param name="request">The DTO containing the updated facility data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the model state is invalid.</response>
        /// <response code="404">If the facility is not found.</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFacility([FromBody] PutFacilityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingFacility = await context.Facilities.Where(f => f.Id == request.FacilityId).FirstOrDefaultAsync();
            var facilityType = await context.FacilityTypes.Where(ft => ft.Id == request.FacilityTypeId).FirstOrDefaultAsync();

            if (existingFacility == null || facilityType == null)
                return NotFound();

            existingFacility.Name = request.Name;
            existingFacility.Location = request.Location;
            existingFacility.FacilityTypeId = request.FacilityTypeId;

            context.Facilities.Update(existingFacility);
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a facility by its ID.
        /// </summary>
        /// <param name="id">The ID of the facility to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="404">If the facility is not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFacility(Guid id)
        {
            var facility = await context.Facilities.FindAsync(id);
            if (facility == null)
                return NotFound();
            
            context.Facilities.Remove(facility);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}