using ShareMyAdventures.Domain.Events;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

/// <summary>
/// Represents a position update for a participant in the ShareMyAdventures domain.
/// Managed through the <see cref="Participant"/> aggregate root.
/// </summary>
public sealed class Position : BaseAuditableEntity
{
    /// <summary>
    /// Gets the timestamp of the position update.
    /// </summary>
    public string? TimeStamp { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the participant is moving.
    /// </summary>
    public bool IsMoving { get; private set; }

    /// <summary>
    /// Gets the unique identifier for this position update, if provided.
    /// </summary>
    public string? Uuid { get; private set; }

    /// <summary>
    /// Gets the latitude coordinate of the position, if provided.
    /// </summary>
    public double? Latitude { get; private set; }

    /// <summary>
    /// Gets the longitude coordinate of the position, if provided.
    /// </summary>
    public double? Longitude { get; private set; }

    /// <summary>
    /// Gets the speed of the participant in meters per second, if provided.
    /// </summary>
    public double? Speed { get; private set; }

    /// <summary>
    /// Gets the heading direction in degrees, if provided.
    /// </summary>
    public double? Heading { get; private set; }

    /// <summary>
    /// Gets the altitude above sea level in meters, if provided.
    /// </summary>
    public double? Altitude { get; private set; }

    /// <summary>
    /// Gets the type of activity associated with this position, if provided (e.g., walking, running).
    /// </summary>
    public string? ActivityType { get; private set; }

    /// <summary>
    /// Gets the battery level of the device reporting this position, if provided (0.0 to 1.0).
    /// </summary>
    public double? BatteryLevel { get; private set; }

    /// <summary>
    /// Gets the odometer reading in meters, if provided.
    /// </summary>
    public double? Odometer { get; private set; }

    /// <summary>
    /// Gets a string representation of the location, if provided.
    /// </summary>
    public string? Location { get; private set; }

    /// <summary>
    /// Gets the ID of the participant associated with this position.
    /// </summary>
    public string ParticipantId { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the participant associated with this position.
    /// </summary>
    public Participant Participant { get; private set; } = null!;

    // EF Core parameterless constructor
    private Position() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Position"/> class and raises a domain event.
    /// </summary>
    /// <param name="participantId">The ID of the participant.</param>
    /// <param name="latitude">The latitude coordinate.</param>
    /// <param name="longitude">The longitude coordinate.</param>
    /// <param name="timeStamp">The timestamp of the position update.</param>
    /// <param name="isMoving">Whether the participant is moving.</param>
    /// <param name="uuid">The unique identifier for this position, if provided.</param>
    /// <param name="speed">The speed in meters per second, if provided.</param>
    /// <param name="heading">The heading direction in degrees, if provided.</param>
    /// <param name="altitude">The altitude above sea level in meters, if provided.</param>
    /// <param name="activityType">The type of activity, if provided.</param>
    /// <param name="batteryLevel">The battery level (0.0 to 1.0), if provided.</param>
    /// <param name="odometer">The odometer reading in meters, if provided.</param>
    /// <param name="location">A string representation of the location, if provided.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="participantId"/> is null.</exception>
    public Position(
        string participantId,
        double? latitude,
        double? longitude,
        string? timeStamp = null,
        bool isMoving = false,
        string? uuid = null,
        double? speed = null,
        double? heading = null,
        double? altitude = null,
        string? activityType = null,
        double? batteryLevel = null,
        double? odometer = null,
        string? location = null)
    {
        ParticipantId = participantId ?? throw new ArgumentNullException(nameof(participantId));
        Latitude = latitude;
        Longitude = longitude;
        TimeStamp = timeStamp;
        IsMoving = isMoving;
        Uuid = uuid;
        Speed = speed;
        Heading = heading;
        Altitude = altitude;
        ActivityType = activityType;
        BatteryLevel = batteryLevel;
        Odometer = odometer;
        Location = location;

        AddDomainEvent(new PositionEvent(this));
    }
}