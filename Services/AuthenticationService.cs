using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.Services
{
    public static class AuthenticationService
    {
        public const string AuthErrorKey = "AuthError";

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtIssuer = configuration["Jwt:Issuer"];
            var jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:Key is missing in configuration");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateLifetime = true,
                 ValidateAudience = false,
                 ValidateIssuerSigningKey = true,
                 ValidIssuer = jwtIssuer,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
             };
            //  options.Events = new JwtBearerEvents
            //  {
            //      OnAuthenticationFailed = context =>
            //     {
            //         context.HttpContext.Items[AuthErrorKey] = context.Exception;
            //         return Task.CompletedTask;
            //     }
            //  };
         });
         
        }
    }
}
