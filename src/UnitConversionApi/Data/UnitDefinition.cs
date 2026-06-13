using System;

namespace UnitConversionApi.Data;

/// <summary>
/// Defines a unit of measurement with its conversion rules relative to its category's base unit.
/// </summary>
public class UnitDefinition
{
    /// <summary>
    /// Gets the name of the unit in lowercase.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the measurement category (e.g., length, temperature).
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Gets a value indicating whether this unit uses a linear conversion factor.
    /// </summary>
    public bool IsFactorBased { get; }

    /// <summary>
    /// Gets the factor to convert from this unit to the base unit (only for factor-based units).
    /// </summary>
    public double ToBaseFactor { get; }

    /// <summary>
    /// Gets the delegate to convert a value from this unit to the base unit (only for non-linear units like temperature).
    /// </summary>
    public Func<double, double>? ToBaseFunc { get; }

    /// <summary>
    /// Gets the delegate to convert a value from the base unit to this unit (only for non-linear units like temperature).
    /// </summary>
    public Func<double, double>? FromBaseFunc { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitDefinition"/> class for factor-based units.
    /// </summary>
    public UnitDefinition(string name, string category, double toBaseFactor)
    {
        Name = name;
        Category = category;
        IsFactorBased = true;
        ToBaseFactor = toBaseFactor;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitDefinition"/> class for function-based units.
    /// </summary>
    public UnitDefinition(string name, string category, Func<double, double> toBaseFunc, Func<double, double> fromBaseFunc)
    {
        Name = name;
        Category = category;
        IsFactorBased = false;
        ToBaseFactor = 1.0;
        ToBaseFunc = toBaseFunc;
        FromBaseFunc = fromBaseFunc;
    }
}
