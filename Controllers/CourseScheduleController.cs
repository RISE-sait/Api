using Api.Attributes;
using Api.Database;
using Api.helpers;
using Api.Mappers;
using Api.Model.CourseSchedules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffTypeEnum = Api.Enums.StaffType;

namespace Api.Controllers
{
    [ApiController]
    [AuthorizeRoles([StaffTypeEnum.Admin])]
    [Route("api/[controller]")]
    public class CourseScheduleController(AppDbContext context) : ControllerBase
    {

        /// <summary>
        /// Creates a new course schedule.
        /// </summary>
        /// <param name="request">The DTO containing the details of the course schedule to create.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<ActionResult<CourseScheduleResponseDto>> CreateCourseSchedule([FromBody] CreateCourseScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var courseSchedule = request.MapToCourseSchedule();

            if (await ScheduleHelper.IsFacilityScheduleOverlapping(context, courseSchedule.MapToCourseScheduleDateTimes()))
            {
                return Conflict(new { Message = "The course schedule overlaps with an existing schedule." });
            }

            await context.CourseSchedules.AddAsync(courseSchedule);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourseScheduleById), new { courseSchedule }, courseSchedule.MapToCourseScheduleResponse());
        }

        /// <summary>
        /// Retrieves a course schedule by its identifiers.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="startDate">The start date of the course schedule.</param>
        /// <param name="facilityId">The ID of the facility.</param>
        /// <param name="beginTime">The begin time of the course schedule.</param>
        /// <returns>A course schedule if found, otherwise a 404 Not Found response.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(CourseScheduleResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourseScheduleById(Guid courseId, DateOnly startDate, Guid facilityId, TimeOnly beginTime)
        {
            var existingCourseSchedule = await context.CourseSchedules
                            .Where(cs => cs.CourseId == courseId && cs.StartDate == startDate && cs.FacilityId == facilityId && cs.BeginTime == beginTime)
                            .Include(cs => cs.Course)
                            .Include(cs => cs.Coach)
                            .FirstOrDefaultAsync();

            if (existingCourseSchedule == null)
            {
                return NotFound();
            }

            return Ok(existingCourseSchedule.MapToCourseScheduleResponse());
        }


        /// <summary>
        /// Updates an existing course schedule.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="startDate">The start date of the course schedule.</param>
        /// <param name="facilityId">The ID of the facility.</param>
        /// <param name="beginTime">The begin time of the course schedule.</param>
        /// <param name="request">The DTO containing the updated details of the course schedule.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCourseSchedule(Guid courseId, DateOnly startDate, Guid facilityId, TimeOnly beginTime, [FromBody] CreateCourseScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCourseSchedule = await context.CourseSchedules
                .Where(cs => cs.CourseId == courseId && cs.StartDate == startDate && cs.FacilityId == facilityId && cs.BeginTime == beginTime)
                .FirstOrDefaultAsync();

            if (existingCourseSchedule == null)
            {
                return NotFound();
            }

            var updatedScheduleInfo = request.MapToCourseSchedule().MapToCourseScheduleDateTimes();

            if (await ScheduleHelper.IsFacilityScheduleOverlapping(context, updatedScheduleInfo))
            {
                return Conflict(new { Message = "The updated course schedule overlaps with an existing schedule." });
            }

            context.Entry(existingCourseSchedule).CurrentValues.SetValues(request);
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing course schedule.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <param name="startDate">The start date of the course schedule.</param>
        /// <param name="facilityId">The ID of the facility.</param>
        /// <param name="beginTime">The begin time of the course schedule.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>

        [HttpDelete]
        public async Task<IActionResult> DeleteCourseSchedule(Guid courseId, DateOnly startDate, Guid facilityId, TimeOnly beginTime)
        {
            var existingCourseSchedule = await context.CourseSchedules
                            .Where(cs => cs.CourseId == courseId && cs.StartDate == startDate && cs.FacilityId == facilityId && cs.BeginTime == beginTime)
                            .FirstOrDefaultAsync();

            if (existingCourseSchedule == null)
            {
                return NotFound();
            }

            context.CourseSchedules.Remove(existingCourseSchedule);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}