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
    public double Distance { get; private set; }

    /// <summary>
    /// Gets the ID of the participant.
    /// </summary>
    public string ParticipantId { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the ID of the associated adventure.
    /// </summary>
    public long AdventureId { get; private set; }

    /// <summary>
    /// Gets the ID of the access level lookup for the participant.
    /// </summary>
    public int AccessLevelLookupId { get; private set; }

    /// <summary>
    /// Gets the participant entity.
    /// </summary>
    public Participant Participant { get; private set; } = null!;

    /// <summary>
    /// Gets the associated adventure.
    /// </summary>
    public Adventure Adventure { get; private set; } = null!;

    /// <summary>
    /// Gets the access level lookup for the participant.
    /// </summary>
    public AccessLevelLookup AccessLevelLookup { get; private set; } = null!;

    // EF Core parameterless constructor (private for encapsulation)
    private ParticipantAdventure() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticipantAdventure"/> class.
    /// </summary>
    public ParticipantAdventure(string participantId, long adventureId, int accessLevelLookupId, double distance = 0)
    {
        ParticipantId = participantId ?? throw new ArgumentNullException(nameof(participantId));
        AdventureId = adventureId;
        AccessLevelLookupId = accessLevelLookupId;
        Distance = distance;
    }
}