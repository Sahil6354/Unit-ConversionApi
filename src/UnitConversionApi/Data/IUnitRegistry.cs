using System.Collections.Generic;

namespace UnitConversionApi.Data;

/// <summary>
/// Defines the contract for the unit of measurement registry.
/// </summary>
public interface IUnitRegistry
{
    /// <summary>
    /// Attempts to retrieve a unit definition by name.
    /// </summary>
    /// <param name="unitName">The normalized lowercase name of the unit.</param>
    /// <param name="unitDefinition">When this method returns, contains the unit definition if found; otherwise, null.</param>
    /// <returns>True if the unit is supported; otherwise, false.</returns>
    bool TryGetUnit(string unitName, out UnitDefinition? unitDefinition);

    /// <summary>
    /// Gets all supported categories and their corresponding unit names.
    /// </summary>
    /// <returns>A dictionary mapping category names to lists of unit names.</returns>
    Dictionary<string, List<string>> GetSupportedUnits();
}
