using Api.Attributes;
using Api.Database;
using Api.Mappers;
using Api.Model.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffTypeEnum = Api.Enums.StaffType;

namespace Api.Controllers
{
    /// <summary>
    /// Controller for managing courses.
    /// </summary>
    /// <param name="context">The database context.</param>
    // [AuthorizeRoles([StaffTypeEnum.Admin])]
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController(AppDbContext context) : ControllerBase
    {

        /// <summary>
        /// Retrieves courses.
        /// </summary>
        /// <returns>The courses </returns>
        /// <response code="200">Returns the courses.</response>
        /// <response code="404">If the course is not found.</response>

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CourseResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCourses()
        {
            var courses = context.Courses.Select(c => c.MapToCourseResponse()).ToList();

            return Ok(courses);
        }

        /// <summary>
        /// Retrieves a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course.</param>
        /// <returns>The course with the specified ID.</returns>
        /// <response code="200">Returns the course with the specified ID.</response>
        /// <response code="404">If the course is not found.</response>

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CourseResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCourseById(Guid id)
        {
            var course = context.Courses.SingleOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course.MapToCourseResponse());
        }

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="request">The data transfer object containing the course details.</param>
        /// <returns>The created course.</returns>
        /// <response code="201">Returns the newly created course.</response>
        /// <response code="400">If the model state is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CourseResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = request.MapToCourse();
            await context.Courses.AddAsync(course);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, course.MapToCourseResponse());
        }

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="request">The data transfer object containing the updated course details.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the model state is invalid.</response>
        /// <response code="404">If the course is not found.</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateCourse([FromBody] UpdateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(ModelState);
            }

            var existingCourse = context.Courses.FirstOrDefault(c => c.Id == request.Id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            existingCourse.Name = request.Name;
            existingCourse.StartDateTime = request.StartDateTime;
            existingCourse.EndDateTime = request.EndDateTime;
            existingCourse.Description = request.Description;

            context.Courses.Update(existingCourse);
            context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletes a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the deletion is successful.</response>
        /// <response code="404">If the course is not found.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCourse(Guid id)
        {
            var course = context.Courses.SingleOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            context.Courses.Remove(course);
            context.SaveChanges();

            return NoContent();
        }
    }
}