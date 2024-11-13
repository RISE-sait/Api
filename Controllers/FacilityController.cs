using Api.Database;
using Api.Mappers;
using Api.Model.Facilities;
using Api.Model.Facilities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacilityController(AppDbContext context) : ControllerBase
    {

        /// <summary>
        /// Gets all facilities.
        /// </summary>
        /// <returns>A list of all facilities.</returns>
        /// <response code="200">Returns a list of all facilities.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Facility>), StatusCodes.Status200OK)]
        public IActionResult GetAllFacilities(int limit = 5)
        {
            var facilities = context.Facilities.Take(limit).ToList();
            return Ok(facilities);
        }

        /// <summary>
        /// Gets a facility by its ID.
        /// </summary>
        /// <param name="id">The ID of the facility to retrieve.</param>
        /// <returns>The facility with the specified ID.</returns>
        /// <response code="200">Returns the facility with the specified ID.</response>
        /// <response code="404">If the facility is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Facility), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFacilityById(Guid id)
        {
            var facility = await context.Facilities.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }

            return Ok(facility);
        }

        /// <summary>
        /// Creates a new facility.
        /// </summary>
        /// <param name="createFacilityDto">The DTO containing the facility creation data.</param>
        /// <returns>The created facility.</returns>
        /// <response code="201">Returns the newly created facility.</response>
        /// <response code="400">If the model state is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Facility), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFacility([FromBody] CreateFacilityDto createFacilityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Facility facility = createFacilityDto.MapToFacility();

            await context.Facilities.AddAsync(facility);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateFacility), new { id = facility.Id }, facility);
        }

        /// <summary>
        /// Updates an existing facility.
        /// </summary>
        /// <param name="id">The ID of the facility to update.</param>
        /// <param name="updateFacilityDto">The DTO containing the updated facility data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the model state is invalid.</response>
        /// <response code="404">If the facility is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFacility(Guid id, [FromBody] CreateFacilityDto updateFacilityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingFacility = await context.Facilities.FindAsync(id);
            if (existingFacility == null)
            {
                return NotFound();
            }

            existingFacility.Name = updateFacilityDto.Name;
            existingFacility.Location = updateFacilityDto.Location;

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
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFacility(Guid id)
        {
            var facility = await context.Facilities.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }

            context.Facilities.Remove(facility);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}