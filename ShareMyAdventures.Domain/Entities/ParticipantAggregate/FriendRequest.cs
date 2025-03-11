using ShareMyAdventures.Domain.Entities;
using ShareMyAdventures.Domain.Events;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

/// <summary>
/// Represents a friend request between two participants in the ShareMyAdventures domain.
/// Managed through the <see cref="Participant"/> aggregate root.
/// </summary>
public sealed class FriendRequest : BaseAuditableEntity
{
    /// <summary>
    /// Gets the ID of the requesting participant.
    /// </summary>
    public string ParticipantId { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the ID of the participant being requested as a friend.
    /// </summary>
    public string ParticipantFriendId { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the requesting participant.
    /// </summary>
    public Participant Participant { get; private set; } = null!;

    /// <summary>
    /// Gets the participant being requested as a friend.
    /// </summary>
    public Participant ParticipantFriend { get; private set; } = null!;

    /// <summary>
    /// Gets the status of the friend request.
    /// </summary>
    public InvitationStatusLookup InvitationStatusLookup { get; private set; } = null!;

    // EF Core parameterless constructor
    private FriendRequest() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FriendRequest"/> class.
    /// </summary>
    /// <param name="participantId">The ID of the requesting participant.</param>
    /// <param name="participantFriendId">The ID of the participant being requested.</param>
    /// <param name="invitationStatusLookup">The initial status of the friend request.</param>
    /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
    public FriendRequest(string participantId, string participantFriendId, InvitationStatusLookup invitationStatusLookup)
    {
        ParticipantId = participantId ?? throw new ArgumentNullException(nameof(participantId));
        ParticipantFriendId = participantFriendId ?? throw new ArgumentNullException(nameof(participantFriendId));
        InvitationStatusLookup = invitationStatusLookup ?? throw new ArgumentNullException(nameof(invitationStatusLookup));
    }

    /// <summary>
    /// Updates the status of the friend request and raises domain events as needed.
    /// </summary>
    /// <param name="statusLookup">The new status of the friend request.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="statusLookup"/> is null.</exception>
    public void UpdateStatus(InvitationStatusLookup statusLookup)
    {
        ArgumentNullException.ThrowIfNull(statusLookup, nameof(statusLookup));

        if (statusLookup.Id == InvitationStatusLookup.Pending.Id &&
            InvitationStatusLookup.Id != statusLookup.Id)
        {
            AddDomainEvent(new FriendInvitationPendingEvent(this));
        }
        else if (statusLookup.Id == InvitationStatusLookup.Accepted.Id &&
                 InvitationStatusLookup.Id != statusLookup.Id)
        {
            AddDomainEvent(new FriendInvitationAcceptedEvent(this));
        }

        InvitationStatusLookup = statusLookup;
    }
}