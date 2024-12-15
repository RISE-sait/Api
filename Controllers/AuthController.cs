using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AppDbContext dbContext, IConfiguration configuration) : ControllerBase
    {
        private const string JwtKeyConfig = "Jwt:Key";
        private const string JwtIssuerConfig = "Jwt:Issuer";
        
        [HttpPost("exchange-jwt")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            try
            {
                var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return BadRequest("JWT token must include an email claim.");

                var staff = dbContext.Staffs.Include(s => s.StaffType).FirstOrDefault(s => s.Email == email);

                if (staff == default)
                    return BadRequest("Staff with associated email not found.");

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Email, email),
                    new(ClaimTypes.Role, staff.StaffType.Name)
                };

                var token = GenerateJwtToken(claims);

                return Ok(new { token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "An unexpected error occurred.", details = ex.Message });
            }
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[JwtKeyConfig] ?? throw new InvalidOperationException("JWT key not configured.")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration[JwtIssuerConfig],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}