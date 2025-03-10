using ShareMyAdventures.Domain.Events;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

public sealed class FriendRequest : BaseAuditableEntity
{
    public string ParticipantId { get; set; } = string.Empty;
    public string ParticipantFriendId { get; set; } = string.Empty;

    public Participant Participant { get; set; } = null!;
    public Participant ParticipantFriend { get; set; } = null!;
    public InvitationStatusLookup InvitationStatusLookup { get; set; } = null!;

    private int _invitationStatusLookupId;
    public int InvitationStatusLookupId
    {
        get => _invitationStatusLookupId;
        set
        {
            if (_invitationStatusLookupId == Enums.InvitationStatusLookups.Pending.Id)
            {
                AddDomainEvent(new FriendInvitationPendingEvent(this));
            }
            if (_invitationStatusLookupId == Enums.InvitationStatusLookups.Accepted.Id)
            {
                AddDomainEvent(new FriendInvitationAcceptedEvent(this));
            }

            _invitationStatusLookupId = value;
        }
    }

}
