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
    /// Gets the access level lookup for the participant.
    /// </summary>
    public AccessLevelLookup AccessLevelLookup { get; private set; } = null!; 

    /// <summary>
    /// Gets the participant entity.
    /// </summary>
    public Participant Participant { get; private set; } = null!;

    /// <summary>
    /// Gets the associated adventure.
    /// </summary>
    public Adventure Adventure { get; private set; } = null!;

    // EF Core parameterless constructor (private for encapsulation)
    private ParticipantAdventure() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParticipantAdventure"/> class.
    /// </summary>
    /// <param name="participantId">The ID of the participant.</param>
    /// <param name="accessLevelLookup">The access level for the participant.</param>
    /// <param name="distance">The distance covered by the participant (default is 0).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="participantId"/> or <paramref name="accessLevelLookup"/> is null.</exception>
    public ParticipantAdventure(string participantId, AccessLevelLookup accessLevelLookup, double distance = 0)
    {
        ParticipantId = participantId ?? throw new ArgumentNullException(nameof(participantId));
        AdventureId = this.Id;
        AccessLevelLookup = accessLevelLookup ?? throw new ArgumentNullException(nameof(accessLevelLookup));
        Distance = distance;
    }
}