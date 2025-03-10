using MediatR;

namespace ShareMyAdventures.Domain.SeedWork;

/// <summary>
/// Abstract base class for domain events in the ShareMyAdventures domain.
/// Serves as a marker for events that can be published and handled within the domain,
/// decoupling the domain model from specific mediator implementations.
/// </summary>
public abstract class BaseEvent : INotification
{
}