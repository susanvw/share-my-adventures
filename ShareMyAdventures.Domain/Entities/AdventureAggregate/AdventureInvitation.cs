using ShareMyAdventures.Domain.Events;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

public sealed class AdventureInvitation : BaseAuditableEntity
{
    public string Email { get; set; } = string.Empty;
    public long AdventureId { get; set; }
    public int AccessLevelLookupId { get; set; }

    private int _invitationStatusLookupId;
    public int InvitationStatusLookupId
    {
        get => _invitationStatusLookupId;
        set
        {
            if (_invitationStatusLookupId == Enums.InvitationStatusLookups.Pending.Id)
            {
                AddDomainEvent(new AdventureInvitationPendingEvent(this));
            }

            _invitationStatusLookupId = value;
        }
    }

    public AccessLevelLookup AccessLevelLookup { get; set; } = null!;
    public Adventure Adventure { get; set; } = null!;
    public InvitationStatusLookup InvitationStatusLookup { get; set; } = null!;
}
