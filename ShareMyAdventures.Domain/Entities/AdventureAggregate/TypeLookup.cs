using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

/// <summary>
/// Represents a type lookup as a value object in the ShareMyAdventures domain.
/// Indicates the category or nature of an adventure.
/// </summary>
public sealed class TypeLookup : ValueObject
{
    /// <summary>
    /// Gets the unique identifier for this type.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the name of this type.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeLookup"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the type.</param>
    /// <param name="name">The name of the type.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
    public TypeLookup(int id, string name)
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

    // Predefined static instances aligned with TypeLookups enum
    public static readonly TypeLookup Run = new(1, "Run");
    public static readonly TypeLookup Walk = new(2, "Walk");
    public static readonly TypeLookup Cycle = new(3, "Cycle");
    public static readonly TypeLookup RoadTrip = new(4, "Road Trip");
    public static readonly TypeLookup Gathering = new(5, "Gathering");

    /// <summary>
    /// Gets all predefined type lookups.
    /// </summary>
    public static IEnumerable<TypeLookup> All => [Run, Walk, Cycle, RoadTrip, Gathering];
}