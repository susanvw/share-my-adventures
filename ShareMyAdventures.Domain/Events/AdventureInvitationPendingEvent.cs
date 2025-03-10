using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Events;

public sealed class AdventureInvitationPendingEvent(AdventureInvitation item) : BaseEvent
{
    public AdventureInvitation Item { get; } = item;
}

