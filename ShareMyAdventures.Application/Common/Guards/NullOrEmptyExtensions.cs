// API Project: Guards/NullOrEmptyExtensions.cs
namespace ShareMyAdventures.Application.Common.Guards;

/// <summary>
/// Provides extension methods for guarding against null or empty values.
/// </summary>
public static class NullOrEmptyExtensions
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the string is null or empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="paramName">The name of the parameter for the exception message.</param>
    /// <returns>The original string if it is not null or empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the string is null or empty.</exception>
    public static string ThrowIfNullOrEmpty(this string? value, string paramName)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(paramName);

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the DateTime? has no value.
    /// </summary>
    /// <param name="value">The nullable DateTime to check.</param>
    /// <param name="paramName">The name of the parameter for the exception message.</param>
    /// <returns>The value of the nullable DateTime if it has a value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the nullable DateTime has no value.</exception>
    public static DateTime ThrowIfHasNoValue(this DateTime? value, string paramName)
    {
        if (!value.HasValue)
            throw new ArgumentNullException(paramName);

        return value.Value;
    }

    // Optional: Add more guard methods
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the string is null, empty, or whitespace.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="paramName">The name of the parameter for the exception message.</param>
    /// <returns>The original string if it is not null, empty, or whitespace.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the string is null, empty, or whitespace.</exception>
    public static string ThrowIfNullOrWhiteSpace(this string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(paramName);

        return value;
    }
}