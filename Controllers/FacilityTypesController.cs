using Api.Database;
using Api.Mappers;
using Api.Model.Facilities;
using Api.Model.Facilities.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FacilityTypesController(AppDbContext _context) : ControllerBase
    {
        /// <summary>
        /// Creates a new facility type.
        /// </summary>
        /// <param name="facilityType">The facility type to create.</param>
        /// <returns>The created facility type.</returns>
        /// <response code="201">Returns the newly created facility type.</response>
        /// <response code="400">If the model state is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FacilityType), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFacilityType([FromBody] CreateFacilityTypeRequest createFacilityTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await _context.FacilityTypes.AnyAsync(ft => ft.Name == createFacilityTypeDto.Name);
            if (exists)
            {
                return Conflict(new { Message = "A facility type with the same name already exists." });
            }

            FacilityType facilityType = createFacilityTypeDto.MapToFacilityType();

            await _context.FacilityTypes.AddAsync(facilityType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFacilityTypeById), new { id = facilityType.Id }, facilityType);
        }

        /// <summary>
        /// Gets a facility type by its ID.
        /// </summary>
        /// <param name="id">The ID of the facility type to retrieve.</param>
        /// <returns>The facility type with the specified ID.</returns>
        /// <response code="200">Returns the facility type with the specified ID.</response>
        /// <response code="404">If the facility type is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FacilityType), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFacilityTypeById(Guid id)
        {
            var facilityType = await _context.FacilityTypes.Where(ft => ft.Id == id).FirstOrDefaultAsync();

            if (facilityType == null)
            {
                return NotFound();
            }

            return Ok(facilityType);
        }

        /// <summary>
        /// Gets all facility types.
        /// </summary>
        /// <returns>A list of all facility types.</returns>
        /// <response code="200">Returns a list of all facility types.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FacilityType>), StatusCodes.Status200OK)]
        public IActionResult GetAllFacilityTypes()
        {
            var facilityTypes = _context.FacilityTypes.ToList();
            return Ok(facilityTypes);
        }

        /// <summary>
        /// Updates an existing facility type.
        /// </summary>
        /// <param name="id">The ID of the facility type to update.</param>
        /// <param name="facilityType">The updated facility type data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the model state is invalid.</response>
        /// <response code="404">If the facility type is not found.</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFacilityType([FromBody] UpdateFacilityTypeDto updateFacilityTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingFacilityType = await _context.FacilityTypes.FindAsync(updateFacilityTypeDto.Id);
            if (existingFacilityType == null)
            {
                return NotFound();
            }

            existingFacilityType.Name = updateFacilityTypeDto.Name;

            _context.FacilityTypes.Update(existingFacilityType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a facility type by its ID.
        /// </summary>
        /// <param name="id">The ID of the facility type to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="404">If the facility type is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFacilityType(Guid id)
        {
            var facilityType = await _context.FacilityTypes.FindAsync(id);
            if (facilityType == null)
            {
                return NotFound();
            }

            _context.FacilityTypes.Remove(facilityType);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}