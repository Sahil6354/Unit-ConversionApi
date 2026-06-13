using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitConversionApi.Data;
using UnitConversionApi.Models;
using Xunit;

namespace UnitConversionApi.Tests.Controllers;

/// <summary>
/// Contains integration tests for the ConversionsController.
/// </summary>
public class ConversionsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConversionsControllerTests"/> class.
    /// </summary>
    /// <param name="factory">The web application factory fixture.</param>
    public ConversionsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Tests that POST /api/conversions with a valid request returns 200 OK and correct result.
    /// </summary>
    [Fact]
    public async Task PostConversion_ValidRequest_ReturnsOkAndResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 100.0,
            FromUnit = Units.Celsius,
            ToUnit = Units.Fahrenheit
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/conversions", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var data = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        Assert.NotNull(data);
        Assert.Equal(100.0, data.InputValue);
        Assert.Equal(Units.Celsius, data.FromUnit);
        Assert.Equal(Units.Fahrenheit, data.ToUnit);
        Assert.InRange(data.Result, 212.0 - 0.001, 212.0 + 0.001);
        Assert.Equal(Units.CategoryTemperature, data.Category);
    }

    /// <summary>
    /// Tests that POST /api/conversions with an invalid unit returns 400 Bad Request and details.
    /// </summary>
    [Fact]
    public async Task PostConversion_InvalidUnit_ReturnsBadRequestAndError()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new ConversionRequest
        {
            Value = 10.0,
            FromUnit = "xyz",
            ToUnit = Units.Meter
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/conversions", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errorData = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        Assert.NotNull(errorData);
        Assert.True(errorData.ContainsKey("error"));
        Assert.True(errorData.ContainsKey("supportedUnits"));
        Assert.Equal("Unsupported unit: 'xyz'", errorData["error"].ToString());
    }

    /// <summary>
    /// Tests that GET /api/conversions/units returns 200 OK and the dictionary of supported units.
    /// </summary>
    [Fact]
    public async Task GetSupportedUnits_ReturnsOkAndSupportedUnits()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/conversions/units");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var units = await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
        Assert.NotNull(units);
        Assert.Contains(Units.CategoryLength, units.Keys);
        Assert.Contains(Units.CategoryTemperature, units.Keys);
        Assert.Contains(Units.CategoryWeight, units.Keys);

        // Verify some registered unit names are present
        Assert.Contains(Units.Meter, units[Units.CategoryLength]);
        Assert.Contains(Units.Celsius, units[Units.CategoryTemperature]);
        Assert.Contains(Units.Kilogram, units[Units.CategoryWeight]);
    }
}
