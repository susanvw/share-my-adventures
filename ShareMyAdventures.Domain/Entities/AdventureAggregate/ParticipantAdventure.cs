using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

/// <summary>
/// Represents a participant's involvement in an adventure.
/// Managed exclusively through the <see cref="Adventure"/> aggregate root.
/// </summary>
public sealed class ParticipantAdventure : BaseAuditableEntity
{
    /// <summary>
    /// Gets the distance covered by the participant in the adventure.
    /// </summary>
    public double Distance { get; set; }

    /// <summary>
    /// Gets the ID of the participant.
    /// </summary>
    public string ParticipantId { get;  set; } = string.Empty;

    /// <summary>
    /// Gets the ID of the associated adventure.
    /// </summary>
    public long AdventureId { get; set; }

    /// <summary>
    /// Gets the ID of the access level lookup for the participant.
    /// </summary>
    public int AccessLevelLookupId { get; set; }

    /// <summary>
    /// Gets the participant entity.
    /// </summary>
    public Participant Participant { get;  set; } = null!;

    /// <summary>
    /// Gets the associated adventure.
    /// </summary>
    public Adventure Adventure { get;  set; } = null!;

    /// <summary>
    /// Gets the access level lookup for the participant.
    /// </summary>
    public AccessLevelLookup AccessLevelLookup { get;  set; } = null!;
}