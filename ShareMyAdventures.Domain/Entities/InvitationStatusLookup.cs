using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities;

/// <summary>
/// Represents an invitation status lookup as a value object in the ShareMyAdventures domain.
/// Indicates the status of a friend request or adventure invitation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="InvitationStatusLookup"/> class.
/// </remarks>
/// <param name="id">The unique identifier for the status.</param>
/// <param name="name">The name of the status.</param>
/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
public sealed class InvitationStatusLookup(int id, string name) : ValueObject
{
    /// <summary>
    /// Gets the unique identifier for this invitation status.
    /// </summary>
    public int Id { get; } = id;

    /// <summary>
    /// Gets the name of this invitation status.
    /// </summary>
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    /// <summary>
    /// Defines the components that determine equality for this value object.
    /// </summary>
    /// <returns>An enumerable of objects used for equality comparison.</returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return Name;
    }

    // Predefined static instances aligned with InvitationStatusLookups enum
    public static readonly InvitationStatusLookup Pending = new(1, "Pending");
    public static readonly InvitationStatusLookup Accepted = new(2, "Accepted");
    public static readonly InvitationStatusLookup Rejected = new(3, "Rejected");

    /// <summary>
    /// Gets all predefined invitation status lookups.
    /// </summary>
    public static IEnumerable<InvitationStatusLookup> All => [Pending, Accepted, Rejected];
}