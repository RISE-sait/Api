using Api.Database;
using Api.helpers;
using dotenv.net;
using Scalar.AspNetCore;

DotEnv.Load();

var IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new GuidJsonConverter());
});

builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.

// if (IsDevelopment)
// {
    app.MapOpenApi();
    app.MapScalarApiReference();
// }

app.MapGet("/", () => "hey");

app.Run();