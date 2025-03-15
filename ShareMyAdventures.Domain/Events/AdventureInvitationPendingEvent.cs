using ShareMyAdventures.Domain.Entities.InvitationAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Events;

/// <summary>
/// Represents a domain event triggered when a new adventure invitation is pending for a user.
/// </summary>
public sealed class AdventureInvitationPendingEvent : BaseEvent
{
    /// <summary>
    /// Gets the adventure invitation associated with this event.
    /// </summary>
    public AdventureInvitation Item { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AdventureInvitationPendingEvent"/> class.
    /// </summary>
    /// <param name="item">The adventure invitation that is pending.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null.</exception>
    public AdventureInvitationPendingEvent(AdventureInvitation item)
    {
        ArgumentNullException.ThrowIfNull(item);
        Item = item;
    }
}