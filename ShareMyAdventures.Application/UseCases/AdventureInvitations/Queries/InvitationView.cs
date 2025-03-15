using ShareMyAdventures.Domain.Entities.InvitationAggregate;
using System.Reflection.Metadata.Ecma335;

namespace ShareMyAdventures.Application.UseCases.AdventureInvitations.Queries;

public sealed record InvitationView
{
    public long Id { get; internal set; } = 0;
    public string Name { get; internal set; } = string.Empty;
    public DateTime StartDate { get; internal set; }
    public DateTime EndDate { get; internal set; }
    public string? MeetUp { get; internal set; }
    public double? MeetUpLatitude { get; internal set; }
    public double? MeetUpLongitude { get; internal set; }
    public string? Destination { get; internal set; }
    public double? DestinationLatitude { get; internal set; }
    public double? DestinationLongitude { get; internal set; }
    public long StatusLookupNameId { get; internal set; } = 0;
    public string StatusLookupName { get; internal set; } = string.Empty;
    public int TypeLookupId { get; internal set; } = 0;
    public string TypeLookupName { get; internal set; } = string.Empty;
    public string? AccessLevelLookupName { get; internal set; }
    public string Email { get; internal set; } = string.Empty;
    public int AccessLevelLookupId { get; internal set; } = 0;

    internal static Func<AdventureInvitation, string, InvitationView> MapFrom => (AdventureInvitation entity, string email) =>
    {
        return new InvitationView
        {

            StatusLookupName = entity.Adventure.StatusLookup.Name,
            TypeLookupName = entity.Adventure.TypeLookup.Name,
            AccessLevelLookupId = entity.AccessLevelLookupId,
            AccessLevelLookupName = entity.AccessLevelLookup.Name,
            Email = email
        };
    };
}