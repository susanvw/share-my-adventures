using ShareMyAdventures.Domain.Entities;
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
    /// <param name="email">The email address of the invited user.</param>
    /// <param name="adventureId">The ID of the associated adventure.</param>
    /// <param name="accessLevelLookup">The access level for the invitation.</param>
    /// <param name="invitationStatusLookup">The initial status of the invitation.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="email"/>, <paramref name="accessLevelLookup"/>, or <paramref name="invitationStatusLookup"/> is null.</exception>
    public AdventureInvitation(string email, long adventureId, AccessLevelLookup accessLevelLookup, InvitationStatusLookup invitationStatusLookup)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        AdventureId = adventureId;
        AccessLevelLookup = accessLevelLookup ?? throw new ArgumentNullException(nameof(accessLevelLookup));
        InvitationStatusLookup = invitationStatusLookup ?? throw new ArgumentNullException(nameof(invitationStatusLookup));
    }

    /// <summary>
    /// Updates the invitation status and raises a domain event if transitioning to 'Pending'.
    /// </summary>
    /// <param name="statusLookup">The new status lookup.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="statusLookup"/> is null.</exception>
    public void UpdateStatus(InvitationStatusLookup statusLookup)
    {
        ArgumentNullException.ThrowIfNull(statusLookup, nameof(statusLookup));

        if (statusLookup.Id == InvitationStatusLookup.Pending.Id &&
            InvitationStatusLookup.Id != statusLookup.Id)
        {
            AddDomainEvent(new AdventureInvitationPendingEvent(this));
        }
        InvitationStatusLookup = statusLookup;
    }
}