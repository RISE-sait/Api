using Api.Database;
using Api.Mappers;
using Api.Model.People.Employees;
using Api.Model.People.Employees.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoachController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Coach>> Get()
        {
            var coaches = context.Coaches.ToList();
            return Ok(coaches);
        }

        [HttpGet("{id}")]
        public ActionResult<Coach> GetById(Guid id)
        {
            var coach = context.Coaches.Find(id);
            if (coach == null)
            {
                return NotFound(new { Message = "Coach not found" });
            }
            return Ok(coach);
        }

        [HttpPost]
        public ActionResult<Coach> Post([FromBody] CreateCoachDto createCoachDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var coach = createCoachDto.MapToCoach();

            context.Coaches.Add(coach);
            context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = coach.Id }, coach);
        }

        [HttpPut("{id}")]
        public ActionResult<Coach> Put(Guid id, [FromBody] Coach coach)
        {
            if (id != coach.Id)
            {
                return BadRequest();
            }

            var existingCoach = context.Coaches.Find(id);
            if (existingCoach == null)
            {
                return NotFound(new { Message = "Coach not found" });
            }

            context.Entry(existingCoach).CurrentValues.SetValues(coach);
            context.SaveChanges();

            return Ok(coach);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var coach = context.Coaches.Find(id);
            if (coach == null)
            {
                return NotFound(new { Message = "Coach not found" });
            }

            context.Coaches.Remove(coach);
            context.SaveChanges();

            return NoContent();
        }
    }
}
