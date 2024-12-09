using Api.Attributes;
using Api.Database;
using Api.Mappers;
using Api.Model.Courses;
using Api.Model.Memberships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffTypeEnum = Api.Enums.StaffType;

namespace Api.Controllers
{
    /// <summary>
    /// Controller for managing memberships.
    /// </summary>
    /// <param name="context">The database context.</param>
    [AuthorizeRoles([StaffTypeEnum.Admin])]
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController(AppDbContext context) : ControllerBase
    {

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CourseResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMemberships()
        {
            var memberships = context.Memberships.Include(m => m.MembershipPlans).Select(c => c.MapToMembershipResponse()).ToList();

            return Ok(memberships);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MembershipResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMembershipById(Guid id)
        {
            var membership = context.Memberships.SingleOrDefault(c => c.Id == id);
            if (membership == default)
            {
                return NotFound();
            }

            return Ok(membership.MapToMembershipResponse());
        }

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="request">The data transfer object containing the course details.</param>
        /// <returns>The created course.</returns>
        /// <response code="201">Returns the newly created course.</response>
        /// <response code="400">If the model state is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(MembershipResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse([FromBody] CreateMembershipRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membership = request.MapToMembership();
            await context.Memberships.AddAsync(membership);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembershipById), new { id = membership.Id }, membership.MapToMembershipResponse());
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateMembership([FromBody] UpdateMembershipRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(ModelState);
            }

            var membership = context.Memberships.FirstOrDefault(c => c.Id == request.Id);
            if (membership == default)
                return NotFound();

            membership.Name = request.Name;
            membership.StartDateTime = request.StartDateTime;
            membership.EndDateTime = request.EndDateTime;
            membership.Description = request.Description;
            membership.Price = request.Price;

            context.Memberships.Update(membership);
            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteMembership(Guid id)
        {
            var membership = context.Memberships.SingleOrDefault(c => c.Id == id);
            if (membership == default)
                return NotFound();

            context.Memberships.Remove(membership);
            context.SaveChanges();

            return NoContent();
        }
    }
}