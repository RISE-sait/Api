using System.Linq;
using Api.Database;
using Api.Mappers;
using Api.Model.Memberships;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipPlanController(AppDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<MembershipPlan>> GetMembershipPlans()
        {
            var plans = dbContext.MembershipPlans
                .Include(mp => mp.Membership)
                .Select(mp => mp.MapToMembershipPlanResponse())
                .ToList();

            return Ok(plans);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<MembershipPlanResponseDto> GetMembershipPlan(Guid id)
        {
            try
            {
                var plan = dbContext.MembershipPlans
                    .Include(p => p.Membership)
                    .FirstOrDefault(p => p.Id == id);

                if (plan == null)
                    return NotFound();

                return plan.MapToMembershipPlanResponse();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<MembershipPlan>> CreateMembershipPlan(CreateMembershipPlanDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plan = request.ToMembershipPlan();

            dbContext.MembershipPlans.Add(plan);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMembershipPlan),
                new { id = plan.Id },
                plan
            );
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMembershipPlan(Guid id, CreateMembershipPlanDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var plan = request.ToMembershipPlan();

            if (id != plan.Id)
                return BadRequest();

            var existingPlan = dbContext.MembershipPlans.Include(mp => mp.Membership).FirstOrDefault(mp => mp.Id == id);
            if (existingPlan == default)
                return NotFound();

            existingPlan.Name = plan.Name;
            existingPlan.Price = plan.Price;
            existingPlan.RecurringPaymentPlan = plan.RecurringPaymentPlan;
            existingPlan.MembershipId = plan.MembershipId;

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMembershipPlan(Guid id)
        {
            var plan = await dbContext.MembershipPlans.FindAsync(id);
            if (plan == null)
                return NotFound();

            dbContext.MembershipPlans.Remove(plan);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}