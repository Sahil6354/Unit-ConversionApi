using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Controllers;

/// <summary>
/// Controller for handling unit conversion requests and querying supported units.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConversionsController : ControllerBase
{
    private readonly IConversionService _conversionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConversionsController"/> class.
    /// </summary>
    /// <param name="conversionService">The conversion service.</param>
    public ConversionsController(IConversionService conversionService)
    {
        _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));
    }

    /// <summary>
    /// Converts a numerical value from one unit to another.
    /// </summary>
    /// <param name="request">The conversion request.</param>
    /// <returns>A conversion response with the result.</returns>
    /// <response code="200">Returns the converted value and metadata.</response>
    /// <response code="400">If the input contains invalid units or different categories, or invalid model state.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConversionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Convert([FromBody] ConversionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _conversionService.ConvertAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Gets all supported measurement categories and units.
    /// </summary>
    /// <returns>A dictionary containing categories and their respective supported units.</returns>
    /// <response code="200">Returns the supported units mapping.</response>
    [HttpGet("units")]
    [ProducesResponseType(typeof(Dictionary<string, List<string>>), StatusCodes.Status200OK)]
    public IActionResult GetSupportedUnits()
    {
        var units = _conversionService.GetSupportedUnits();
        return Ok(units);
    }
}
