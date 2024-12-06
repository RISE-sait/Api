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
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController(AppDbContext dbContext, IConfiguration configuration) : ControllerBase
    {
        private const string JwtKeyConfig = "Jwt:Key";
        private const string JwtIssuerConfig = "Jwt:Issuer";
        private const string EnvironmentConfig = "Environment:Environment";

        [HttpPost("exchange-jwt")]
        public IActionResult Login()
        {
            var principal = HttpContext.User;

            if (!principal.Identity?.IsAuthenticated ?? true)
                return BadRequest("Invalid token.");

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return BadRequest("Invalid token: Email claim not found.");

            var staff = dbContext.Staffs.Include(s => s.StaffType).FirstOrDefault(s => s.Email == email);

            if (staff == null)
                return BadRequest("Invalid token: Staff not found.");

            var claims = principal.Claims.Append(new Claim("staffTypeName", staff.StaffType.Name));

            var token = GenerateJwtToken(claims);

            // Set the JWT token as a cookie
            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = configuration[EnvironmentConfig] != "Development",
                SameSite = SameSiteMode.Strict
            });

            // Return the token
            return Ok(new { token });
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