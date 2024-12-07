using Api.Database;
using Api.helpers;
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
services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new GuidJsonConverter());
});

services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.UseAuthentication();
app.MapControllers();

// Configure the HTTP request pipeline.

if (isDevelopment)
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.MapGet("/", () => "hey");

app.Run();