using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnitConversionApi.Data;
using UnitConversionApi.Middleware;
using UnitConversionApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add controllers with JSON options (camelCase, ignore null)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Swagger/OpenAPI setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Unit Conversion API",
        Version = "v1"
    });
});

// Dependency injection configuration
builder.Services.AddSingleton<IUnitRegistry, UnitRegistry>();
builder.Services.AddScoped<IConversionService, ConversionService>();

var app = builder.Build();

// Register global exception middleware BEFORE UseRouting
app.UseGlobalExceptionMiddleware();

// Enable Swagger always (not just in Development)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Unit Conversion API v1");
    options.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// Exposed class structure to allow integration testing using WebApplicationFactory.
/// </summary>
public partial class Program
{
}
