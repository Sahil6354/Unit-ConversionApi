using System.ComponentModel.DataAnnotations;

namespace UnitConversionApi.Models;

/// <summary>
/// Represents a request to convert a value from one unit to another.
/// </summary>
public class ConversionRequest
{
    /// <summary>
    /// Gets or sets the numerical value to be converted.
    /// </summary>
    [Required(ErrorMessage = "Value is required.")]
    public double? Value { get; set; }

    /// <summary>
    /// Gets or sets the source unit of measurement.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "FromUnit is required and cannot be empty.")]
    public string FromUnit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target unit of measurement.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "ToUnit is required and cannot be empty.")]
    public string ToUnit { get; set; } = string.Empty;
}
