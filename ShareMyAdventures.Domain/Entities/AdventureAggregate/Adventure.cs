using ShareMyAdventures.Domain.Entities.LocationAggregate;
using ShareMyAdventures.Domain.Enums;
using ShareMyAdventures.Domain.Events;
using ShareMyAdventures.Domain.SeedWork;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

public sealed class Adventure : BaseAuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public long? MeetupLocationId { get; set; }
    public long? DestinationLocationId { get; set; }
    public int TypeLookupId { get; set; }

    private int _statusLookupId;
    public int StatusLookupId
    {
        get => _statusLookupId;
        set
        {
            if (_statusLookupId == Enums.StatusLookups.InProgress.Id)
            {
                AddDomainEvent(new AdventureStatusInProgressEvent(this));
            }

            _statusLookupId = value;
        }
    }

    public Location? MeetupLocationLookup { get; set; }
    public Location? DestinationLocationLookup { get; set; }
    public TypeLookup TypeLookup { get; set; } = null!;
    public StatusLookup StatusLookup { get; set; } = null!;
    public List<ParticipantAdventure> Participants { get;} = [];
    public List<AdventureInvitation> Invitations { get; } = [];

    public void AddParticipant(ParticipantAdventure participant)
    {
        if(!Participants.Any(x => x.ParticipantId == participant.ParticipantId))
        {
            Participants.Add(participant);
        }
    }

    public void RemoveParticipant(ParticipantAdventure participant)
    {
        if (Participants.Any(x => x.ParticipantId == participant.ParticipantId))
        {
            Participants.Remove(participant);
        }
    }

    public void AddInvitations(AdventureInvitation invitation)
    {
        if (!Invitations.Any(x => x.Id == invitation.Id))
        {
            Invitations.Add(invitation);
        }
    }

    public void RemoveInvitations(AdventureInvitation invitation)
    {
        if (Invitations.Any(x => x.Id == invitation.Id))
        {
            Invitations.Remove(invitation);
        }
    }

    public AdventureInvitation? FindByEmail(string email)
    {
        return Invitations
            .FirstOrDefault(x =>
                x.Email == email &&
                x.InvitationStatusLookupId == InvitationStatusLookups.Pending.Id);
    }
}
