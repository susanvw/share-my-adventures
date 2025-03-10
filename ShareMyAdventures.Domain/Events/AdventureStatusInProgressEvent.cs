using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Events;

public sealed class AdventureStatusInProgressEvent(Adventure item) : BaseEvent
{
    public Adventure Item { get; } = item;
}

