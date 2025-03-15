using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Adventures.Queries;

public sealed class ParticipantAccessView
{
    public required string Id { get;  set; } 
    public required string UserName { get; set; }
    public required string DisplayName { get; set; }
    public string? Photo { get;  set; }
    public required long AccessLevelLookupId { get;  set; }
    public required string AccessLevelLookupName { get;  set; }


    internal static readonly Func<Participant, ParticipantAdventure, ParticipantAccessView> MapFrom = (user, adventure) =>
    {
        return new ParticipantAccessView
        {
            DisplayName = user.DisplayName,
            Id = user.Id,
            Photo = user.Photo,
            UserName = user.UserName!,
            AccessLevelLookupId = adventure.AccessLevelLookup.Id,
            AccessLevelLookupName = adventure.AccessLevelLookup?.Name ?? string.Empty
        };
    };
}
