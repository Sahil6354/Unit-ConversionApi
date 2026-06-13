using System;

namespace UnitConversionApi.Exceptions;

/// <summary>
/// Exception thrown when a conversion request references an unsupported unit or is mathematically invalid.
/// </summary>
public class UnsupportedConversionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnsupportedConversionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UnsupportedConversionException(string message) : base(message)
    {
    }
}
