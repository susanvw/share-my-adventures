using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

/// <summary>
/// Represents an access level lookup as a value object in the ShareMyAdventures domain.
/// Indicates the level of access a participant or invitation has for an adventure.
/// </summary>
public sealed class AccessLevelLookup : ValueObject
{
    /// <summary>
    /// Gets the unique identifier for this access level.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the name of this access level.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessLevelLookup"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the access level.</param>
    /// <param name="name">The name of the access level.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
    public AccessLevelLookup(int id, string name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    /// Defines the components that determine equality for this value object.
    /// </summary>
    /// <returns>An enumerable of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return Name;
    }

    // Predefined static instances for common access levels
    public static readonly AccessLevelLookup Viewer = new(1, "Viewer");
    public static readonly AccessLevelLookup Participant = new(2, "Participant");
    public static readonly AccessLevelLookup Administrator = new(3, "Administrator");

    /// <summary>
    /// Gets all predefined access level lookups.
    /// </summary>
    public static IEnumerable<AccessLevelLookup> All => [Viewer, Participant, Administrator];
}