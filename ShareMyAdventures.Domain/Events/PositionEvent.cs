using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Events;

public sealed class PositionEvent(Position item) : BaseEvent
{
    public Position Item { get; } = item;
}
