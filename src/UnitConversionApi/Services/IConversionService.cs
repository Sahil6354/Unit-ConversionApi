using System.Collections.Generic;
using System.Threading.Tasks;
using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

/// <summary>
/// Defines the contract for unit conversion services.
/// </summary>
public interface IConversionService
{
    /// <summary>
    /// Performs the unit conversion on the specified request.
    /// </summary>
    /// <param name="request">The unit conversion request details.</param>
    /// <returns>A conversion response containing the input values and conversion result.</returns>
    Task<ConversionResponse> ConvertAsync(ConversionRequest request);

    /// <summary>
    /// Gets all supported categories and units of measurement.
    /// </summary>
    /// <returns>A dictionary mapping categories to their list of units.</returns>
    Dictionary<string, List<string>> GetSupportedUnits();
}
