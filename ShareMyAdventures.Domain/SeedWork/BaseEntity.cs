using System.ComponentModel.DataAnnotations.Schema;

namespace ShareMyAdventures.Domain.SeedWork;

/// <summary>
/// Abstract base class for entities in the ShareMyAdventures domain.
/// Represents an object with identity and the ability to raise domain events to signal significant changes.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for this entity.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Backing field for the collection of domain events raised by this entity.
    /// </summary>
    private readonly List<BaseEvent> _domainEvents = [];

    /// <summary>
    /// Gets a read-only collection of domain events associated with this entity.
    /// These events represent significant state changes that need to be communicated.
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to this entity’s collection of events.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="domainEvent"/> is null.</exception>
    public void AddDomainEvent(BaseEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Removes a specific domain event from this entity’s collection of events.
    /// </summary>
    /// <param name="domainEvent">The domain event to remove.</param>
    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        // No null check here as List.Remove silently handles null
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from this entity’s collection.
    /// Typically called after events have been processed or published.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}