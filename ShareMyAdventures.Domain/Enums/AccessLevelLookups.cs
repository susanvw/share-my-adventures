using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Enums;

/// <summary>
/// Represents a type-safe enumeration of access levels within the ShareMyAdventures domain.
/// Defines the possible roles a user can have in relation to an adventure.
/// </summary>
public sealed class AccessLevelLookups : BaseEnum
{
    /// <summary>
    /// Represents a user with read-only access to an adventure.
    /// </summary>
    public static readonly AccessLevelLookups Viewer = new(1, nameof(Viewer));

    /// <summary>
    /// Represents a user who can participate in an adventure.
    /// </summary>
    public static readonly AccessLevelLookups Participant = new(2, nameof(Participant));

    /// <summary>
    /// Represents a user with administrative control over an adventure.
    /// </summary>
    public static readonly AccessLevelLookups Administrator = new(3, nameof(Administrator));

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessLevelLookups"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the access level.</param>
    /// <param name="name">The display name of the access level.</param>
    private AccessLevelLookups(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    /// Gets an array of all defined access levels.
    /// </summary>
    public static AccessLevelLookups[] All => [Viewer, Participant, Administrator];
}