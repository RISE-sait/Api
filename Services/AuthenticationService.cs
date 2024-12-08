using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.Services
{
    public static class AuthenticationService
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtIssuer = configuration["Jwt:Issuer"];
            var jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:Key is missing in configuration");

            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["jwtToken"];
                        if (!string.IsNullOrEmpty(token))
                            context.Token = token;
                        else
                        {
                            var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
                            if (!string.IsNullOrEmpty(authHeader))
                                context.Token = authHeader.Split(" ").Last();
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
