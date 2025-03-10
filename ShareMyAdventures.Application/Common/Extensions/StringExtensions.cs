// Application Project: Extensions/StringExtensions.cs
using System.Text;
using System.Text.RegularExpressions;

namespace ShareMyAdventures.Application.Common.Extensions;

/// <summary>
/// Provides extension methods for working with strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Reverses the characters in the given string.
    /// </summary>
    /// <param name="value">The string to reverse.</param>
    /// <returns>A new string with the characters in reverse order.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
    public static string Reverse(this string value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "Value cannot be null");

        char[] tempArray = value.ToCharArray();
        Array.Reverse(tempArray);
        return new string(tempArray);
    }

    /// <summary>
    /// Checks if the string is null, empty, or consists only of whitespace characters.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>True if the string is null, empty, or whitespace; otherwise, false.</returns>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Truncates the string to the specified maximum length, appending an ellipsis ("...") if truncated.
    /// </summary>
    /// <param name="value">The string to truncate.</param>
    /// <param name="maxLength">The maximum length of the resulting string, including the ellipsis.</param>
    /// <returns>The truncated string, or the original if it’s shorter than maxLength.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxLength"/> is less than 3.</exception>
    public static string Truncate(this string? value, int maxLength)
    {
        if (maxLength < 3)
            throw new ArgumentOutOfRangeException(nameof(maxLength), "Max length must be at least 3 to accommodate ellipsis.");

        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value ?? string.Empty;

        return value.Substring(0, maxLength - 3) + "...";
    }

    /// <summary>
    /// Converts the string to title case (capitalizes the first letter of each word).
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>A new string in title case, or empty if the input is null or empty.</returns>
    public static string ToTitleCase(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
    }

    /// <summary>
    /// Checks if the string matches a specified regular expression pattern.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <param name="pattern">The regular expression pattern to match against.</param>
    /// <returns>True if the string matches the pattern; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pattern"/> is null.</exception>
    public static bool Matches(this string? value, string pattern)
    {
        if (pattern == null)
            throw new ArgumentNullException(nameof(pattern), "Pattern cannot be null");

        if (string.IsNullOrEmpty(value))
            return false;

        return Regex.IsMatch(value, pattern);
    }

    /// <summary>
    /// Removes all whitespace characters from the string.
    /// </summary>
    /// <param name="value">The string to process.</param>
    /// <returns>A new string with all whitespace removed, or empty if the input is null or empty.</returns>
    public static string RemoveWhitespace(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return new string(value.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }

    /// <summary>
    /// Checks if the string is a valid email address format.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>True if the string is a valid email format; otherwise, false.</returns>
    public static bool IsEmail(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        // Simple regex for email validation (can be adjusted for stricter rules)
        return value.Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    /// <summary>
    /// Replaces multiple consecutive occurrences of a character with a single occurrence.
    /// </summary>
    /// <param name="value">The string to process.</param>
    /// <param name="character">The character to collapse.</param>
    /// <returns>A new string with consecutive occurrences of the character collapsed, or empty if the input is null.</returns>
    public static string CollapseCharacter(this string? value, char character)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return Regex.Replace(value, $"{character}+", character.ToString());
    }

    /// <summary>
    /// Ensures the string ends with the specified suffix, appending it if necessary.
    /// </summary>
    /// <param name="value">The string to process.</param>
    /// <param name="suffix">The suffix to ensure.</param>
    /// <returns>A new string ending with the suffix.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="suffix"/> is null.</exception>
    public static string EnsureEndsWith(this string? value, string suffix)
    {
        if (suffix == null)
            throw new ArgumentNullException(nameof(suffix), "Suffix cannot be null");

        if (string.IsNullOrEmpty(value))
            return suffix;

        return value.EndsWith(suffix) ? value : value + suffix;
    }

    /// <summary>
    /// Splits the string into an array based on a delimiter and trims each element.
    /// </summary>
    /// <param name="value">The string to split.</param>
    /// <param name="delimiter">The character to split on.</param>
    /// <returns>An array of trimmed strings, or an empty array if the input is null or empty.</returns>
    public static string[] SplitAndTrim(this string? value, char delimiter)
    {
        if (string.IsNullOrEmpty(value))
            return Array.Empty<string>();

        return value.Split(delimiter)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToArray();
    }

    /// <summary>
    /// Purpose: Converts a string to a URL-friendly slug (e.g., for SEO or routing).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToSlug(this string? value)
    {
        if (string.IsNullOrEmpty(value)) 
            return string.Empty;
        return Regex.Replace(value.ToLower().Trim(), @"[^a-z0-9]+", "-").Trim('-');
    }

    /// <summary>
    /// Purpose: Masks part of a string (e.g., for sensitive data like credit card numbers).
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maskChar"></param>
    /// <param name="visiblePrefix"></param>
    /// <param name="visibleSuffix"></param>
    /// <returns></returns>
    public static string Mask(this string? value, char maskChar = '*', int visiblePrefix = 4, int visibleSuffix = 4)
    {
        if (string.IsNullOrEmpty(value)) 
            return string.Empty;
        if (value.Length <= visiblePrefix + visibleSuffix) 
            return value;
        return value.Substring(0, visiblePrefix) + new string(maskChar, value.Length - visiblePrefix - visibleSuffix) + value.Substring(value.Length - visibleSuffix);
    }

    /// <summary>
    /// Purpose: Simplifies string formatting with object properties (e.g., for logging or messages).
    /// </summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string FormatWith(this string? format, params object[] args)
    {
        return string.IsNullOrEmpty(format) ? string.Empty : string.Format(format, args);
    }

    /// <summary>
    /// Purpose: Checks if a string contains only numeric characters.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNumeric(this string? value)
    {
        return !string.IsNullOrEmpty(value) && value.All(char.IsDigit);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string Decode(this string value)
    {
        // Decode the Base64 encoded token
        byte[] tokenBytes = Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(tokenBytes);
    }
}