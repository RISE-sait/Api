using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacilityController : ControllerBase
    {
        [HttpPost("{facilityId}/booking")]
        public ActionResult ManageBooking(
            Guid facilityId,
            [FromQuery] DateTime startDateTime,
            [FromQuery] DateTime endDateTime)
        {
            try
            {
                ManageBooking(facilityId, startDateTime, endDateTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}