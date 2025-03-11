using ShareMyAdventures.Domain.Entities;
using ShareMyAdventures.Domain.Entities.LocationAggregate;
using ShareMyAdventures.Domain.Events;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

/// <summary>
/// Represents an adventure aggregate root in the ShareMyAdventures domain.
/// Manages participants and invitations as part of its consistency boundary.
/// </summary>
public sealed class Adventure : BaseAuditableEntity, IAggregateRoot
{
    private readonly List<ParticipantAdventure> _participants = [];
    private readonly List<AdventureInvitation> _invitations = [];

    /// <summary>
    /// Gets the name of the adventure.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the start date of the adventure.
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// Gets the end date of the adventure.
    /// </summary>
    public DateTime EndDate { get; private set; }

    /// <summary>
    /// Gets the ID of the meetup location.
    /// </summary>
    public long? MeetupLocationId { get; private set; }

    /// <summary>
    /// Gets the ID of the destination location.
    /// </summary>
    public long? DestinationLocationId { get; private set; }

    /// <summary>
    /// Gets the adventure type lookup.
    /// </summary>
    public TypeLookup TypeLookup { get; private set; } = null!;

    /// <summary>
    /// Gets the adventure status lookup.
    /// </summary>
    public StatusLookup StatusLookup { get; private set; } = null!;

    /// <summary>
    /// Gets the meetup location lookup, if available.
    /// </summary>
    public Location? MeetupLocationLookup { get; private set; }

    /// <summary>
    /// Gets the destination location lookup, if available.
    /// </summary>
    public Location? DestinationLocationLookup { get; private set; }

    /// <summary>
    /// Gets a read-only collection of participants in the adventure.
    /// </summary>
    public IReadOnlyCollection<ParticipantAdventure> Participants => _participants.AsReadOnly();

    /// <summary>
    /// Gets a read-only collection of invitations for the adventure.
    /// </summary>
    public IReadOnlyCollection<AdventureInvitation> Invitations => _invitations.AsReadOnly();

    // EF Core parameterless constructor (private for encapsulation)
    private Adventure() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Adventure"/> class.
    /// </summary>
    /// <param name="name">The name of the adventure.</param>
    /// <param name="startDate">The start date of the adventure.</param>
    /// <param name="endDate">The end date of the adventure.</param>
    /// <param name="typeLookup">The type of the adventure.</param>
    /// <param name="statusLookup">The initial status of the adventure.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/>, <paramref name="typeLookup"/>, or <paramref name="statusLookup"/> is null.</exception>
    public Adventure(string name, DateTime startDate, DateTime endDate, TypeLookup typeLookup, StatusLookup statusLookup)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        StartDate = startDate;
        EndDate = endDate;
        TypeLookup = typeLookup ?? throw new ArgumentNullException(nameof(typeLookup));
        StatusLookup = statusLookup ?? throw new ArgumentNullException(nameof(statusLookup));
    }

    /// <summary>
    /// Updates the status of the adventure and raises a domain event if transitioning to 'InProgress'.
    /// </summary>
    /// <param name="statusLookup">The new status lookup.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="statusLookup"/> is null.</exception>
    public void UpdateStatus(StatusLookup statusLookup)
    {
        ArgumentNullException.ThrowIfNull(statusLookup, nameof(statusLookup));
        if (statusLookup.Id == StatusLookup.InProgress.Id && StatusLookup.Id != statusLookup.Id)
        {
            AddDomainEvent(new AdventureStatusInProgressEvent(this));
        }
        StatusLookup = statusLookup;
    }

    /// <summary>
    /// Adds a participant to the adventure if not already present.
    /// </summary>
    /// <param name="participant">The participant to add.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="participant"/> is null.</exception>
    public void AddParticipant(ParticipantAdventure participant)
    {
        ArgumentNullException.ThrowIfNull(participant, nameof(participant));
        if (!_participants.Any(x => x.ParticipantId == participant.ParticipantId))
        {
            _participants.Add(participant);
        }
    }

    /// <summary>
    /// Removes a participant from the adventure if present.
    /// </summary>
    /// <param name="participant">The participant to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="participant"/> is null.</exception>
    public void RemoveParticipant(ParticipantAdventure participant)
    {
        ArgumentNullException.ThrowIfNull(participant, nameof(participant));
        _participants.Remove(participant);
    }

    /// <summary>
    /// Adds an invitation to the adventure if not already present.
    /// </summary>
    /// <param name="invitation">The invitation to add.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="invitation"/> is null.</exception>
    public void AddInvitation(AdventureInvitation invitation)
    {
        ArgumentNullException.ThrowIfNull(invitation, nameof(invitation));
        if (!_invitations.Any(x => x.Id == invitation.Id))
        {
            _invitations.Add(invitation);
        }
    }

    /// <summary>
    /// Removes an invitation from the adventure if present.
    /// </summary>
    /// <param name="invitation">The invitation to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="invitation"/> is null.</exception>
    public void RemoveInvitation(AdventureInvitation invitation)
    {
        ArgumentNullException.ThrowIfNull(invitation, nameof(invitation));
        _invitations.Remove(invitation);
    }

    /// <summary>
    /// Finds a pending invitation by email address.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>The matching invitation if found; otherwise, null.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="email"/> is null.</exception>
    public AdventureInvitation? FindInvitationByEmail(string email)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        return _invitations.FirstOrDefault(x =>
            x.Email == email &&
            x.InvitationStatusLookup.Id == InvitationStatusLookup.Pending.Id);
    }
}