using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Events;

public sealed class FriendInvitationAcceptedEvent(FriendRequest item) : BaseEvent
{
    public FriendRequest Item { get; } = item;
}

