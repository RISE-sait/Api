using Api.Attributes;
using Api.Database;
using Api.Mappers;
using Api.Model.People.Staff;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffTypeEnum = Api.Enums.StaffType;

namespace Api.Controllers
{
    /// <summary>
    /// Handles HTTP requests related to Coach entities.
    /// </summary>
    [ApiController]
    [AuthorizeRoles([StaffTypeEnum.Admin])]
    [Route("api/[controller]")]
    public class CoachController(AppDbContext context, ILogger<CoachController> logger) : ControllerBase
    {
        /// <summary>
        /// Retrieves all coaches.
        /// </summary>
        /// <returns>A list of coaches.</returns>
        /// <response code="200">Returns the list of coaches</response>
        [HttpGet]
        [AuthorizeRoles([StaffTypeEnum.Coach])]
        [ProducesResponseType(typeof(IEnumerable<StaffResponseDto>), 200)]
        public ActionResult<IEnumerable<StaffResponseDto>> Get()
        {
            var coaches = context.Staffs.Select(s => s.MapToCoachResponse()).ToList();
            return Ok(coaches);
        }

        /// <summary>
        /// Retrieves a coach by ID.
        /// </summary>
        /// <param name="id">The ID of the coach to retrieve.</param>
        /// <returns>The coach with the specified ID.</returns>
        /// <response code="200">Returns the coach with the specified ID</response>
        /// <response code="404">If the coach is not found</response>
        [HttpGet("{id:guid}")]
        [AuthorizeRoles([StaffTypeEnum.Coach])]
        [ProducesResponseType(typeof(StaffResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCoachById(Guid id)
        {

            var coach = context.Staffs.FirstOrDefault(c => c.Id == id);
            if (coach == null)
            {
                return NotFound();
            }
            return Ok(coach.MapToCoachResponse());
        }


        /// <summary>
        /// Creates a new coach.
        /// </summary>
        /// <param name="createCoachDto">The data transfer object containing the details of the coach to create.</param>
        /// <returns>The created coach.</returns>
        /// <response code="201">Returns the newly created coach</response>
        /// <response code="400">If the model state is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(StaffResponseDto), 201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> Post([FromBody] CreateStaffRequest createCoachDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var coach = createCoachDto.MapToCoach();

                await context.Staffs.AddAsync(coach);
                await context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCoachById), new { id = coach.Id }, coach.MapToCoachResponse());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating coach: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating coach");
            }
        }

        /// <summary>
        /// Updates an existing coach.
        /// </summary>
        /// <param name="request">The updated coach object.</param>
        /// <returns>The updated coach.</returns>
        /// <response code="200">Returns the updated coach</response>
        /// <response code="400">If the ID in the URL does not match the ID in the coach object</response>
        /// <response code="404">If the coach is not found</response>
        [HttpPut]
        [AuthorizeRoles([StaffTypeEnum.Coach])]
        [ProducesResponseType(typeof(Staff), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(UpdateStaffRequest request)
        {
            var existingCoach = context.Staffs.FirstOrDefault(c => c.Id == request.Id);
            if (existingCoach == null)
            {
                return NotFound(new { Message = "Coach not found" });
            }

            existingCoach.Name = request.Name;
            existingCoach.PhoneNumber = request.PhoneNumber;
            existingCoach.ProfilePic = request.ProfilePic;
            existingCoach.StaffTypeId = request.StaffTypeId;

            context.Staffs.Update(existingCoach);
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a coach by ID.
        /// </summary>
        /// <param name="id">The ID of the coach to delete.</param>
        /// <response code="204">If the coach is successfully deleted</response>
        /// <response code="404">If the coach is not found</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public ActionResult Delete(Guid id)
        {
            var coach = context.Staffs.Find(id);
            if (coach == null)
            {
                return NotFound(new { Message = "Coach not found" });
            }

            context.Staffs.Remove(coach);
            context.SaveChanges();

            return NoContent();
        }
    }
}
