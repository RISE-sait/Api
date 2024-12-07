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
        private const string EnvironmentConfig = "Environment:Environment";

        [HttpPost("exchange-jwt")]
        [AllowAnonymous]
        public IActionResult Login([FromHeader(Name = "Authorization")] string? authorizationHeader)
        {
            try
            {
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                    return Unauthorized("Missing or invalid Authorization header.");

                var oldToken = authorizationHeader["Bearer ".Length..].Trim();

                var principal = ValidateJwtToken(oldToken);

                var emailClaim = principal.FindFirst(ClaimTypes.Email);
                if (emailClaim == null || string.IsNullOrEmpty(emailClaim.Value))
                    return BadRequest("JWT token must include an email claim.");

                var email = emailClaim.Value;

                var staff = dbContext.Staffs.Include(s => s.StaffType).FirstOrDefault(s => s.Email == email);

                if (staff == null)
                    return BadRequest("Staff with associated email not found.");

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Email, email),
                    new("staffTypeName", staff.StaffType.Name)
                };

                var token = GenerateJwtToken(claims);

                // Set the JWT token as a cookie
                Response.Cookies.Append("jwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = configuration[EnvironmentConfig] != "Development",
                    SameSite = SameSiteMode.Strict
                });

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
        
        private ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration[JwtKeyConfig] ?? throw new InvalidOperationException("JWT key not configured."));

            try
            {
                // Validate the JWT token
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration[JwtIssuerConfig],
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out _);

                return principal;
            }
            catch (SecurityTokenInvalidIssuerException ex)
            {
                throw new UnauthorizedAccessException("Invalid JWT token issuer.", ex);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                throw new UnauthorizedAccessException("JWT token signature is invalid.", ex);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("JWT token is invalid.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during token validation. {ex.Message}");
            }
        }

    }
}