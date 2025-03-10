using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Application.UseCases.Adventures.Queries;

public sealed class AdventureView
{
    public long Id { get; internal set; }
    public string Name { get; internal set; } = string.Empty;
    public DateTime StartDate { get; internal set; }
    public DateTime EndDate { get; internal set; }
    public string? MeetUp { get; internal set; }
    public double? MeetUpLatitude { get; internal set; }
    public double? MeetUpLongitude { get; internal set; }
    public string? FinalDestination { get; internal set; }
    public double? FinalDestinationLatitude { get; internal set; }
    public double? FinalDestinationLongitude { get; internal set; }
    public long StatusLookupId { get; internal set; }
    public string StatusLookupName { get; internal set; } = string.Empty;
    public int TypeLookupId { get; internal set; }
    public string TypeLookupName { get; internal set; } = string.Empty;

    public List<ParticipantAccessView> Participants { get; internal set; } = [];

    internal static Func<Adventure, AdventureView> MapFrom => (Adventure entity) =>
    {
        var adventure = entity;
        var meetup = entity?.MeetupLocationLookup;
        var destination = entity?.DestinationLocationLookup;

        return new AdventureView
        {

            // adventure
            Id = adventure.Id,
            Name = adventure.Name,
            StartDate = adventure.StartDate,
            EndDate = adventure.EndDate,

            // meetup
            MeetUp = meetup?.ToString(),
            MeetUpLatitude = meetup?.Latitude,
            MeetUpLongitude = meetup?.Longitude,

            // destination
            FinalDestination = destination?.ToString(),
            FinalDestinationLatitude = destination?.Latitude,
            FinalDestinationLongitude = destination?.Longitude,

            // status
            StatusLookupId = adventure.StatusLookupId,
            StatusLookupName = adventure.StatusLookup.Name,

            // type
            TypeLookupId = adventure.TypeLookupId,
            TypeLookupName = adventure.TypeLookup.Name,

            // access level
            Participants = [.. adventure.Participants.Select(x => ParticipantAccessView.MapFrom(x.Participant, x))]
        };
    };
}