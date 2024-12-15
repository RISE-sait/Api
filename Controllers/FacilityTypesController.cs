using Api.Attributes;
using Api.Database;
using Api.Mappers;
using Api.Model.Facilities;
using Api.Model.Facilities.Dto;
using Microsoft.AspNetCore.Mvc;
using StaffTypeEnum = Api.Enums.StaffType;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   [AuthorizeRoles([StaffTypeEnum.Admin])]
    public class FacilityTypesController(AppDbContext context) : ControllerBase
    {
        /// <summary>
        /// Creates a new facility type.
        /// </summary>
        /// <param name="request">The facility type to create.</param>
        /// <returns>The created facility type.</returns>
        /// <response code="201">Returns the newly created facility type.</response>
        /// <response code="400">If the model state is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FacilityType), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateFacilityType([FromBody] CreateFacilityTypeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exists = context.FacilityTypes.Any(ft => ft.Name == request.Name);
            if (exists)
            {
                return Conflict(new { Message = "A facility type with the same name already exists." });
            }

            var facilityType = request.MapToFacilityType();

            context.FacilityTypes.Add(facilityType);
            context.SaveChanges();

            return CreatedAtAction(nameof(GetFacilityTypeById), new { id = facilityType.Id }, facilityType);
        }

        /// <summary>
        /// Gets a facility type by its ID.
        /// </summary>
        /// <param name="id">The ID of the facility type to retrieve.</param>
        /// <returns>The facility type with the specified ID.</returns>
        /// <response code="200">Returns the facility type with the specified ID.</response>
        /// <response code="404">If the facility type is not found.</response>
        [HttpGet("{id:guid}")]
        [AuthorizeRoles([StaffTypeEnum.Coach])]
        [ProducesResponseType(typeof(FacilityType), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFacilityTypeById(Guid id)
        {

            var facilityType = context.FacilityTypes.FirstOrDefault(ft => ft.Id == id);

            if (facilityType == null)
                return NotFound();

            return Ok(facilityType);
        }

        /// <summary>
        /// Gets all facility types.
        /// </summary>
        /// <returns>A list of all facility types.</returns>
        /// <response code="200">Returns a list of all facility types.</response>
        [HttpGet]
       // [AuthorizeRoles([StaffTypeEnum.Coach])]
        [ProducesResponseType(typeof(IEnumerable<FacilityType>), StatusCodes.Status200OK)]
        public IActionResult GetAllFacilityTypes()
        {
            var facilityTypes = context.FacilityTypes.ToList();
            return Ok(facilityTypes);
        }

        /// <summary>
        /// Updates an existing facility type.
        /// </summary>
        /// <param name="request">The updated facility type data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the model state is invalid.</response>
        /// <response code="404">If the facility type is not found.</response>
        [HttpPut]
        [AuthorizeRoles([StaffTypeEnum.Coach])]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFacilityType([FromBody] UpdateFacilityTypeDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingFacilityType = await context.FacilityTypes.FindAsync(request.Id);
            if (existingFacilityType == null)
            {
                return NotFound("Facility type not found.");
            }

            existingFacilityType.Name = request.Name;

            context.FacilityTypes.Update(existingFacilityType);
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a facility type by its ID.
        /// </summary>
        /// <param name="id">The ID of the facility type to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="404">If the facility type is not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFacilityType(Guid id)
        {
            var facilityType = await context.FacilityTypes.FindAsync(id);
            if (facilityType == null)
            {
                return NotFound();
            }

            context.FacilityTypes.Remove(facilityType);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}