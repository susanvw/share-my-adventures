// API Project: Guards/ItemNotFoundExtensions.cs

// API Project: Guards/ItemNotFoundExtensions.cs

// API Project: Guards/ItemNotFoundExtensions.cs

// API Project: Guards/ItemNotFoundExtensions.cs
using ShareMyAdventures.Application.Common.Exceptions;

namespace ShareMyAdventures.Application.Common.Guards; // Adjusted to TypeDev.API.Guards for consistency with earlier examples

/// <summary>
/// Provides extension methods for guarding against items not being found.
/// </summary>
public static class ItemNotFoundExtensions
{
    /// <summary>
    /// Throws a <see cref="NotFoundException"/> if the object is null, indicating the item was not found.
    /// </summary>
    /// <typeparam name="T">The type of the object, constrained to reference types.</typeparam>
    /// <param name="obj">The object to check for null.</param>
    /// <param name="identifier">The identifier used to look up the entity (e.g., ID, key).</param>
    /// <returns>The original object if it is not null.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="identifier"/> is null.</exception>
    /// <exception cref="NotFoundException">Thrown if <paramref name="obj"/> is null.</exception>
    /// <example>
    /// <code>
    /// var user = db.Users.Find(id).ThrowIfNotFound("User", id);
    /// </code>
    /// </example>
    public static T ThrowIfNotFound<T>(this T? obj, object identifier) where T : class
    {
        ArgumentNullException.ThrowIfNull(identifier);

        if (obj == null)
            throw new NotFoundException(nameof(T), identifier);

        return obj;
    }
}