using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

/// <summary>
/// Represents a status lookup as a value object in the ShareMyAdventures domain.
/// Indicates the current state of an adventure.
/// </summary>
public sealed class StatusLookup : ValueObject
{
    /// <summary>
    /// Gets the unique identifier for this status.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the name of this status.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusLookup"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the status.</param>
    /// <param name="name">The name of the status.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
    public StatusLookup(int id, string name)
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

    // Predefined static instances aligned with StatusLookups enum
    public static readonly StatusLookup Created = new(1, "Created");
    public static readonly StatusLookup InProgress = new(2, "In Progress");
    public static readonly StatusLookup Completed = new(3, "Completed");

    /// <summary>
    /// Gets all predefined status lookups.
    /// </summary>
    public static IEnumerable<StatusLookup> All => [Created, InProgress, Completed];
}