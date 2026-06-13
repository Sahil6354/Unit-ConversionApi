namespace UnitConversionApi.Models;

/// <summary>
/// Represents the result of a unit conversion request.
/// </summary>
public class ConversionResponse
{
    /// <summary>
    /// Gets or sets the original input value that was converted.
    /// </summary>
    public double InputValue { get; set; }

    /// <summary>
    /// Gets or sets the source unit of measurement.
    /// </summary>
    public string FromUnit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target unit of measurement.
    /// </summary>
    public string ToUnit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the converted result value, rounded to 6 decimal places.
    /// </summary>
    public double Result { get; set; }

    /// <summary>
    /// Gets or sets the measurement category (e.g., temperature, length).
    /// </summary>
    public string Category { get; set; } = string.Empty;
}
