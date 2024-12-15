using Api.Database;
using Api.helpers;
// using Api.Middlewares;
using Api.Services;
using dotenv.net;
using Scalar.AspNetCore;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())  // Ensure the correct directory is used
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Load appsettings.json
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true) // Environment-specific settings
    .AddJsonFile("secrets.json", optional: true, reloadOnChange: true) // Environment-specific settings
    .AddEnvironmentVariables();

var isDevelopment = configuration["Environment:Environment"] == "Development";

// Add services to the container.

services.AddAuthentication(configuration);
services.AddStripeServices(configuration);
services.AddEndpointsApiExplorer();
services.AddOpenApi();
services.AddHttpClient<HubSpotService>();
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new GuidJsonConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapGet("/", () => "hey");

if (isDevelopment)
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
// app.UseMiddleware<JwtExceptionMiddleware>();

app.MapControllers();

app.Run();