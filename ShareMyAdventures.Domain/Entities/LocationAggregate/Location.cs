using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.LocationAggregate;

/// <summary>
/// Represents a physical location in the ShareMyAdventures domain.
/// Serves as an aggregate root for storing user-added locations that can be referenced by adventures.
/// </summary>
public sealed class Location : BaseAuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Gets the primary address line of the location (e.g., street number and name).
    /// </summary>
    public string AddressLine1 { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the secondary address line of the location (e.g., apartment or suite number).
    /// </summary>
    public string AddressLine2 { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the suburb or neighborhood of the location, if applicable.
    /// </summary>
    public string? Suburb { get; private set; }

    /// <summary>
    /// Gets the city or town of the location, if applicable.
    /// </summary>
    public string? City { get; private set; }

    /// <summary>
    /// Gets the province, state, or region of the location, if applicable.
    /// </summary>
    public string? Province { get; private set; }

    /// <summary>
    /// Gets the postal code or ZIP code of the location, if applicable.
    /// </summary>
    public string? PostalCode { get; private set; }

    /// <summary>
    /// Gets the country of the location.
    /// </summary>
    public string Country { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the latitude coordinate of the location, if provided.
    /// </summary>
    public double? Latitude { get; private set; }

    /// <summary>
    /// Gets the longitude coordinate of the location, if provided.
    /// </summary>
    public double? Longitude { get; private set; }

    // EF Core parameterless constructor (private for encapsulation)
    private Location() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Location"/> class.
    /// </summary>
    /// <param name="addressLine1">The primary address line (e.g., street number and name).</param>
    /// <param name="addressLine2">The secondary address line (e.g., apartment or suite number).</param>
    /// <param name="suburb">The suburb or neighborhood, if applicable.</param>
    /// <param name="city">The city or town, if applicable.</param>
    /// <param name="province">The province, state, or region, if applicable.</param>
    /// <param name="postalCode">The postal code or ZIP code, if applicable.</param>
    /// <param name="country">The country of the location.</param>
    /// <param name="latitude">The latitude coordinate, if provided.</param>
    /// <param name="longitude">The longitude coordinate, if provided.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="addressLine1"/> or <paramref name="country"/> is null.</exception>
    public Location(
        string addressLine1,
        string addressLine2,
        string? suburb,
        string? city,
        string? province,
        string? postalCode,
        string country,
        double? latitude = null,
        double? longitude = null)
    {
        AddressLine1 = addressLine1 ?? throw new ArgumentNullException(nameof(addressLine1));
        AddressLine2 = addressLine2 ?? string.Empty; // Allow empty but not null
        Suburb = suburb;
        City = city;
        Province = province;
        PostalCode = postalCode;
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Updates the location details.
    /// </summary>
    /// <param name="addressLine1">The new primary address line.</param>
    /// <param name="addressLine2">The new secondary address line.</param>
    /// <param name="suburb">The new suburb, if applicable.</param>
    /// <param name="city">The new city, if applicable.</param>
    /// <param name="province">The new province, if applicable.</param>
    /// <param name="postalCode">The new postal code, if applicable.</param>
    /// <param name="country">The new country.</param>
    /// <param name="latitude">The new latitude, if provided.</param>
    /// <param name="longitude">The new longitude, if provided.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="addressLine1"/> or <paramref name="country"/> is null.</exception>
    public void Update(
        string addressLine1,
        string? addressLine2 = null,
        string? suburb = null,
        string? city = null,
        string? province = null,
        string? postalCode = null,
        string? country = null,
        double? latitude = null,
        double? longitude = null)
    {
        AddressLine1 = addressLine1 ?? throw new ArgumentNullException(nameof(addressLine1));
        AddressLine2 = addressLine2 ?? string.Empty;
        Suburb = suburb;
        City = city;
        Province = province;
        PostalCode = postalCode;
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Latitude = latitude;
        Longitude = longitude;
    }
}