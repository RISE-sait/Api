using Api.Database;
using Api.Mappers;
using Api.Model.Courses;
using Api.Model.Courses.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController(AppDbContext context) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var course = await context.Courses.SingleAsync(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

       [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto createCourseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Course course = createCourseDto.MapToCourse();
            await context.Courses.AddAsync(course);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateCourse), new { id = course.Id }, course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await context.Courses.SingleAsync(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            context.Courses.Remove(course);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}