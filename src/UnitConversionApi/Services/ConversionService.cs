using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitConversionApi.Data;
using UnitConversionApi.Exceptions;
using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

/// <summary>
/// Service that implements unit conversion logic using the registered units.
/// </summary>
public class ConversionService : IConversionService
{
    private readonly IUnitRegistry _unitRegistry;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConversionService"/> class.
    /// </summary>
    /// <param name="unitRegistry">The unit registry containing supported units.</param>
    public ConversionService(IUnitRegistry unitRegistry)
    {
        _unitRegistry = unitRegistry ?? throw new ArgumentNullException(nameof(unitRegistry));
    }

    /// <inheritdoc />
    public Task<ConversionResponse> ConvertAsync(ConversionRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Value == null)
        {
            throw new ArgumentException("Value is required.", nameof(request));
        }

        string fromUnitNormalized = request.FromUnit.Trim().ToLowerInvariant();
        string toUnitNormalized = request.ToUnit.Trim().ToLowerInvariant();

        if (!_unitRegistry.TryGetUnit(fromUnitNormalized, out var fromUnitDef) || fromUnitDef == null)
        {
            throw new UnsupportedConversionException($"Unsupported unit: '{request.FromUnit}'");
        }

        if (!_unitRegistry.TryGetUnit(toUnitNormalized, out var toUnitDef) || toUnitDef == null)
        {
            throw new UnsupportedConversionException($"Unsupported unit: '{request.ToUnit}'");
        }

        if (!fromUnitDef.Category.Equals(toUnitDef.Category, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnsupportedConversionException("Cannot convert between different measurement categories");
        }

        double rawResult;

        if (fromUnitNormalized == toUnitNormalized)
        {
            rawResult = request.Value.Value;
        }
        else
        {
            double resultInBase = fromUnitDef.IsFactorBased
                ? request.Value.Value * fromUnitDef.ToBaseFactor
                : fromUnitDef.ToBaseFunc!(request.Value.Value);

            rawResult = toUnitDef.IsFactorBased
                ? resultInBase / toUnitDef.ToBaseFactor
                : toUnitDef.FromBaseFunc!(resultInBase);
        }

        double roundedResult = Math.Round(rawResult, 6);

        var response = new ConversionResponse
        {
            InputValue = request.Value.Value,
            FromUnit = fromUnitNormalized,
            ToUnit = toUnitNormalized,
            Result = roundedResult,
            Category = fromUnitDef.Category
        };

        return Task.FromResult(response);
    }

    /// <inheritdoc />
    public Dictionary<string, List<string>> GetSupportedUnits()
    {
        return _unitRegistry.GetSupportedUnits();
    }
}
