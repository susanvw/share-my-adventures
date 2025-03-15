using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Application.UseCases.Adventures.Queries;

public sealed class AdventureView
{
    public long Id { get; set; }
    public required string Name { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? MeetUp { get; set; }
    public double? MeetUpLatitude { get; set; }
    public double? MeetUpLongitude { get; set; }
    public string? FinalDestination { get; set; }
    public double? FinalDestinationLatitude { get; set; }
    public double? FinalDestinationLongitude { get; set; }
    public long StatusLookupId { get; set; }
    public required string StatusLookupName { get; set; } 
    public int TypeLookupId { get; set; }
    public required string TypeLookupName { get; set; } 

    public List<ParticipantAccessView> Participants { get; set; } = [];

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
            StatusLookupId = adventure.StatusLookup.Id,
            StatusLookupName = adventure.StatusLookup.Name,

            // type
            TypeLookupId = adventure.TypeLookup.Id,
            TypeLookupName = adventure.TypeLookup.Name,

            // access level
            Participants = [.. adventure.Participants.Select(x => ParticipantAccessView.MapFrom(x.Participant, x))]
        };
    };
}