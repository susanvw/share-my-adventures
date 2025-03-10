using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Events;

public sealed class FriendInvitationPendingEvent(FriendRequest item) : BaseEvent
{
    public FriendRequest Item { get; } = item;
}

