using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    // [AuthorizeRoles([StaffType.Admin])]
    [Route("api/[controller]")]
    public class CustomerController(HubSpotService hubSpotService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await hubSpotService.GetCustomers();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path,

                });
            }
        }
    }
}