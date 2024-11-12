using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Interfaces;
using Api.Model.Courses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        [HttpPatch("{courseId}/schedule")]
        public ActionResult AmendCourseSchedule(Guid courseId, [FromBody] ScheduleInfo[] newSchedule)
        {
            try
            {
                AmendCourseSchedule(courseId, newSchedule);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}