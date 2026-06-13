using System.Threading.Tasks;
using UnitConversionApi.Data;
using UnitConversionApi.Exceptions;
using UnitConversionApi.Models;
using UnitConversionApi.Services;
using Xunit;

namespace UnitConversionApi.Tests.Services;

/// <summary>
/// Contains unit tests for <see cref="ConversionService"/>.
/// </summary>
public class ConversionServiceTests
{
    private readonly IConversionService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConversionServiceTests"/> class.
    /// </summary>
    public ConversionServiceTests()
    {
        IUnitRegistry registry = new UnitRegistry();
        _service = new ConversionService(registry);
    }

    /// <summary>
    /// Tests length conversion from meter to centimeter.
    /// </summary>
    [Fact]
    public async Task ConvertAsync_Length_MeterToCentimeter_ReturnsCorrectValue()
    {
        // Arrange
        var request = new ConversionRequest
        {
            Value = 1.0,
            FromUnit = Units.Meter,
            ToUnit = Units.Centimeter
        };
        double expected = 100.0;

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.Equal(Units.CategoryLength, result.Category);
        Assert.InRange(result.Result, expected - 0.001, expected + 0.001);
    }

    /// <summary>
    /// Tests temperature conversion from celsius to fahrenheit.
    /// </summary>
    [Fact]
    public async Task ConvertAsync_Temperature_CelsiusToFahrenheit_ReturnsCorrectValue()
    {
        // Arrange
        var request = new ConversionRequest
        {
            Value = 0.0,
            FromUnit = Units.Celsius,
            ToUnit = Units.Fahrenheit
        };
        double expected = 32.0;

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.Equal(Units.CategoryTemperature, result.Category);
        Assert.InRange(result.Result, expected - 0.001, expected + 0.001);
    }

    /// <summary>
    /// Tests temperature conversion from celsius to kelvin.
    /// </summary>
    [Fact]
    public async Task ConvertAsync_Temperature_CelsiusToKelvin_ReturnsCorrectValue()
    {
        // Arrange
        var request = new ConversionRequest
        {
            Value = 100.0,
            FromUnit = Units.Celsius,
            ToUnit = Units.Kelvin
        };
        double expected = 373.15;

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.Equal(Units.CategoryTemperature, result.Category);
        Assert.InRange(result.Result, expected - 0.001, expected + 0.001);
    }

    /// <summary>
    /// Tests weight conversion from kilogram to gram.
    /// </summary>
    [Fact]
    public async Task ConvertAsync_Weight_KilogramToGram_ReturnsCorrectValue()
    {
        // Arrange
        var request = new ConversionRequest
        {
            Value = 1.0,
            FromUnit = Units.Kilogram,
            ToUnit = Units.Gram
        };
        double expected = 1000.0;

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.Equal(Units.CategoryWeight, result.Category);
        Assert.InRange(result.Result, expected - 0.001, expected + 0.001);
    }

    /// <summary>
    /// Tests same unit conversion (identity property).
    /// </summary>
    [Fact]
    public async Task ConvertAsync_SameUnit_MeterToMeter_ReturnsSameValue()
    {
        // Arrange
        var request = new ConversionRequest
        {
            Value = 5.0,
            FromUnit = Units.Meter,
            ToUnit = Units.Meter
        };
        double expected = 5.0;

        // Act
        var result = await _service.ConvertAsync(request);

        // Assert
        Assert.Equal(Units.CategoryLength, result.Category);
        Assert.InRange(result.Result, expected - 0.001, expected + 0.001);
    }

    /// <summary>
    /// Tests that converting an invalid fromUnit throws UnsupportedConversionException.
    /// </summary>
    [Fact]
    public async Task ConvertAsync_InvalidFromUnit_ThrowsUnsupportedConversionException()
    {
        // Arrange
        var request = new ConversionRequest
        {
            Value = 10.0,
            FromUnit = "xyz",
            ToUnit = Units.Meter
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnsupportedConversionException>(() => _service.ConvertAsync(request));
        Assert.Contains("Unsupported unit: 'xyz'", exception.Message);
    }

    /// <summary>
    /// Tests that cross-category conversion throws UnsupportedConversionException with correct category error message.
    /// </summary>
    [Fact]
    public async Task ConvertAsync_CrossCategory_ThrowsUnsupportedConversionException()
    {
        // Arrange
        var request = new ConversionRequest
        {
            Value = 10.0,
            FromUnit = Units.Meter,
            ToUnit = Units.Celsius
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnsupportedConversionException>(() => _service.ConvertAsync(request));
        Assert.Equal("Cannot convert between different measurement categories", exception.Message);
    }

    /// <summary>
    /// Tests that GetSupportedUnits returns all 6 categories.
    /// </summary>
    [Fact]
    public void GetSupportedUnits_ReturnsAllSixCategories()
    {
        // Act
        var categories = _service.GetSupportedUnits();

        // Assert
        Assert.Equal(6, categories.Count);
        Assert.Contains(Units.CategoryLength, categories.Keys);
        Assert.Contains(Units.CategoryTemperature, categories.Keys);
        Assert.Contains(Units.CategoryWeight, categories.Keys);
        Assert.Contains(Units.CategoryArea, categories.Keys);
        Assert.Contains(Units.CategoryVolume, categories.Keys);
        Assert.Contains(Units.CategorySpeed, categories.Keys);
    }
}
