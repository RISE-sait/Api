using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        [HttpPost("{teamId}/customer/{customerId}")]
        public ActionResult AddCustomerToTeam(Guid teamId, Guid customerId)
        {
            try
            {
                AddCustomerToTeam(customerId, teamId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/{teamId}/customer/{customerId}")]
        public ActionResult RemoveCustomerFromTeam(Guid teamId, Guid customerId)
        {
            try
            {
                RemoveCustomerFromTeam(customerId, teamId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}