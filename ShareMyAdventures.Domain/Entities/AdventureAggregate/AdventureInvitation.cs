using ShareMyAdventures.Domain.Enums;
using ShareMyAdventures.Domain.Events;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

/// <summary>
/// Represents an invitation to participate in an adventure.
/// Managed exclusively through the <see cref="Adventure"/> aggregate root.
/// </summary>
public sealed class AdventureInvitation : BaseAuditableEntity
{
    /// <summary>
    /// Gets the email address of the invited user.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the ID of the associated adventure.
    /// </summary>
    public long AdventureId { get; private set; }

    /// <summary>
    /// Gets the ID of the access level lookup for the invitation.
    /// </summary>
    public int AccessLevelLookupId { get; private set; }

    /// <summary>
    /// Gets the ID of the invitation status lookup.
    /// </summary>
    public int InvitationStatusLookupId { get; private set; }

    /// <summary>
    /// Gets the access level lookup for the invitation.
    /// </summary>
    public AccessLevelLookup AccessLevelLookup { get; private set; } = null!;

    /// <summary>
    /// Gets the associated adventure.
    /// </summary>
    public Adventure Adventure { get; private set; } = null!;

    /// <summary>
    /// Gets the invitation status lookup.
    /// </summary>
    public InvitationStatusLookup InvitationStatusLookup { get; private set; } = null!;

    // EF Core parameterless constructor (private for encapsulation)
    private AdventureInvitation() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AdventureInvitation"/> class.
    /// </summary>
    public AdventureInvitation(string email, long adventureId, int accessLevelLookupId, int invitationStatusLookupId)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        AdventureId = adventureId;
        AccessLevelLookupId = accessLevelLookupId;
        InvitationStatusLookupId = invitationStatusLookupId;
    }

    /// <summary>
    /// Updates the invitation status and raises a domain event if transitioning to 'Pending'.
    /// </summary>
    /// <param name="statusLookupId">The new status lookup ID.</param>
    public void UpdateStatus(int statusLookupId)
    {
        if (statusLookupId == InvitationStatusLookups.Pending.Id && InvitationStatusLookupId != statusLookupId)
        {
            AddDomainEvent(new AdventureInvitationPendingEvent(this));
        }
        InvitationStatusLookupId = statusLookupId;
    }
}