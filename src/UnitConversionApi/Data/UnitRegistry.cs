using System;
using System.Collections.Generic;

namespace UnitConversionApi.Data;

/// <summary>
/// Contains constant string definitions for all supported units to avoid magic strings.
/// </summary>
public static class Units
{
    /// <summary>Length category name.</summary>
    public const string CategoryLength = "length";

    /// <summary>Temperature category name.</summary>
    public const string CategoryTemperature = "temperature";

    /// <summary>Weight category name.</summary>
    public const string CategoryWeight = "weight";

    /// <summary>Area category name.</summary>
    public const string CategoryArea = "area";

    /// <summary>Volume category name.</summary>
    public const string CategoryVolume = "volume";

    /// <summary>Speed category name.</summary>
    public const string CategorySpeed = "speed";

    /// <summary>Meter unit.</summary>
    public const string Meter = "meter";

    /// <summary>Kilometer unit.</summary>
    public const string Kilometer = "kilometer";

    /// <summary>Centimeter unit.</summary>
    public const string Centimeter = "centimeter";

    /// <summary>Mile unit.</summary>
    public const string Mile = "mile";

    /// <summary>Foot unit.</summary>
    public const string Foot = "foot";

    /// <summary>Inch unit.</summary>
    public const string Inch = "inch";

    /// <summary>Yard unit.</summary>
    public const string Yard = "yard";

    /// <summary>Celsius unit.</summary>
    public const string Celsius = "celsius";

    /// <summary>Fahrenheit unit.</summary>
    public const string Fahrenheit = "fahrenheit";

    /// <summary>Kelvin unit.</summary>
    public const string Kelvin = "kelvin";

    /// <summary>Kilogram unit.</summary>
    public const string Kilogram = "kilogram";

    /// <summary>Gram unit.</summary>
    public const string Gram = "gram";

    /// <summary>Pound unit.</summary>
    public const string Pound = "pound";

    /// <summary>Ounce unit.</summary>
    public const string Ounce = "ounce";

    /// <summary>Tonne unit.</summary>
    public const string Tonne = "tonne";

    /// <summary>Square meter unit.</summary>
    public const string SquareMeter = "squaremeter";

    /// <summary>Square kilometer unit.</summary>
    public const string SquareKilometer = "squarekilometer";

    /// <summary>Square feet unit.</summary>
    public const string SquareFeet = "squarefeet";

    /// <summary>Square mile unit.</summary>
    public const string SquareMile = "squaremile";

    /// <summary>Hectare unit.</summary>
    public const string Hectare = "hectare";

    /// <summary>Acre unit.</summary>
    public const string Acre = "acre";

    /// <summary>Liter unit.</summary>
    public const string Liter = "liter";

    /// <summary>Milliliter unit.</summary>
    public const string Milliliter = "milliliter";

    /// <summary>Gallon unit.</summary>
    public const string Gallon = "gallon";

    /// <summary>Quart unit.</summary>
    public const string Quart = "quart";

    /// <summary>Pint unit.</summary>
    public const string Pint = "pint";

    /// <summary>Cup unit.</summary>
    public const string Cup = "cup";

    /// <summary>Cubic meter unit.</summary>
    public const string CubicMeter = "cubicmeter";

    /// <summary>Meters per second unit.</summary>
    public const string MetersPerSecond = "meterspersecond";

    /// <summary>Kilometers per hour unit.</summary>
    public const string KilometersPerHour = "kilometersperhour";

    /// <summary>Miles per hour unit.</summary>
    public const string MilesPerHour = "milesperhour";

    /// <summary>Knot unit.</summary>
    public const string Knot = "knot";
}

/// <summary>
/// Registry containing all supported measurement units and their conversion configurations.
/// </summary>
public class UnitRegistry : IUnitRegistry
{
    private readonly Dictionary<string, UnitDefinition> _units = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<string>> _categories = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitRegistry"/> class and registers all units.
    /// </summary>
    public UnitRegistry()
    {
        InitializeRegistry();
    }

    private void InitializeRegistry()
    {
        // Length (base: meter)
        Register(new UnitDefinition(Units.Meter, Units.CategoryLength, 1.0));
        Register(new UnitDefinition(Units.Kilometer, Units.CategoryLength, 1000.0));
        Register(new UnitDefinition(Units.Centimeter, Units.CategoryLength, 0.01));
        Register(new UnitDefinition(Units.Mile, Units.CategoryLength, 1609.344));
        Register(new UnitDefinition(Units.Foot, Units.CategoryLength, 0.3048));
        Register(new UnitDefinition(Units.Inch, Units.CategoryLength, 0.0254));
        Register(new UnitDefinition(Units.Yard, Units.CategoryLength, 0.9144));

        // Temperature (base: celsius)
        Register(new UnitDefinition(Units.Celsius, Units.CategoryTemperature, 
            toBaseFunc: c => c, 
            fromBaseFunc: c => c));

        Register(new UnitDefinition(Units.Fahrenheit, Units.CategoryTemperature, 
            toBaseFunc: f => (f - 32.0) * 5.0 / 9.0, 
            fromBaseFunc: c => (c * 9.0 / 5.0) + 32.0));

        Register(new UnitDefinition(Units.Kelvin, Units.CategoryTemperature, 
            toBaseFunc: k => k - 273.15, 
            fromBaseFunc: c => c + 273.15));

        // Weight (base: kilogram)
        Register(new UnitDefinition(Units.Kilogram, Units.CategoryWeight, 1.0));
        Register(new UnitDefinition(Units.Gram, Units.CategoryWeight, 0.001));
        Register(new UnitDefinition(Units.Pound, Units.CategoryWeight, 0.453592));
        Register(new UnitDefinition(Units.Ounce, Units.CategoryWeight, 0.0283495));
        Register(new UnitDefinition(Units.Tonne, Units.CategoryWeight, 1000.0));

        // Area (base: squaremeter)
        Register(new UnitDefinition(Units.SquareMeter, Units.CategoryArea, 1.0));
        Register(new UnitDefinition(Units.SquareKilometer, Units.CategoryArea, 1000000.0));
        Register(new UnitDefinition(Units.SquareFeet, Units.CategoryArea, 0.092903));
        Register(new UnitDefinition(Units.SquareMile, Units.CategoryArea, 2589988.11));
        Register(new UnitDefinition(Units.Hectare, Units.CategoryArea, 10000.0));
        Register(new UnitDefinition(Units.Acre, Units.CategoryArea, 4046.856));

        // Volume (base: liter)
        Register(new UnitDefinition(Units.Liter, Units.CategoryVolume, 1.0));
        Register(new UnitDefinition(Units.Milliliter, Units.CategoryVolume, 0.001));
        Register(new UnitDefinition(Units.Gallon, Units.CategoryVolume, 3.78541));
        Register(new UnitDefinition(Units.Quart, Units.CategoryVolume, 0.946353));
        Register(new UnitDefinition(Units.Pint, Units.CategoryVolume, 0.473176));
        Register(new UnitDefinition(Units.Cup, Units.CategoryVolume, 0.236588));
        Register(new UnitDefinition(Units.CubicMeter, Units.CategoryVolume, 1000.0));

        // Speed (base: meterspersecond)
        Register(new UnitDefinition(Units.MetersPerSecond, Units.CategorySpeed, 1.0));
        Register(new UnitDefinition(Units.KilometersPerHour, Units.CategorySpeed, 0.277778));
        Register(new UnitDefinition(Units.MilesPerHour, Units.CategorySpeed, 0.44704));
        Register(new UnitDefinition(Units.Knot, Units.CategorySpeed, 0.514444));
    }

    private void Register(UnitDefinition unit)
    {
        _units[unit.Name] = unit;

        if (!_categories.TryGetValue(unit.Category, out var categoryList))
        {
            categoryList = new List<string>();
            _categories[unit.Category] = categoryList;
        }
        categoryList.Add(unit.Name);
    }

    /// <inheritdoc />
    public bool TryGetUnit(string unitName, out UnitDefinition? unitDefinition)
    {
        if (string.IsNullOrWhiteSpace(unitName))
        {
            unitDefinition = null;
            return false;
        }

        return _units.TryGetValue(unitName, out unitDefinition);
    }

    /// <inheritdoc />
    public Dictionary<string, List<string>> GetSupportedUnits()
    {
        var result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var category in _categories)
        {
            result[category.Key] = new List<string>(category.Value);
        }
        return result;
    }
}
