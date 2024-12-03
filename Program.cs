using Api.Database;
using Api.helpers;
using Api.Services;
using dotenv.net;
using Scalar.AspNetCore;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var jwtIssuer = configuration["Jwt:Issuer"];
var jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");

var IsDevelopment = configuration["Environment:Environment"] == "Development";

// Add services to the container.

services.AddAuthenticationServices(configuration);

services.AddAuthorizationBuilder()
.AddPolicy("RequireAdmin", policy =>
        policy.RequireClaim("staffTypeName", "Admin"))
.AddPolicy("RequireSuperAdmin", policy =>
    policy.RequireClaim("staffTypeName", "SuperAdmin"))
.AddPolicy("RequireCoach", policy =>
    policy.RequireClaim("staffTypeName", "Coach"));

services.AddEndpointsApiExplorer();
services.AddOpenApi();
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new GuidJsonConverter());
});

builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Configure the HTTP request pipeline.

if (IsDevelopment)
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/", () => "hey");

app.Run();