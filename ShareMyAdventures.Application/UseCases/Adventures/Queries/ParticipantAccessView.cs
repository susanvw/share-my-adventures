using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Adventures.Queries;

public sealed class ParticipantAccessView
{
    public string Id { get; internal set; } = null!;
    public string UserName { get; set; } = null!;
    public string DisplayName { get; internal set; } = null!;
    public string? Photo { get; internal set; }
    public long AccessLevelLookupId { get; internal set; }
    public string AccessLevelLookupName { get; internal set; } = null!;
  

    internal static ParticipantAccessView MapFrom( Participant user, ParticipantAdventure adventure)
    { 
        return new ParticipantAccessView
        {
            DisplayName = user.DisplayName,
            Id = user.Id,
            Photo = user.Photo,
            UserName = user.UserName!,
            AccessLevelLookupId = adventure.AccessLevelLookupId,
            AccessLevelLookupName = adventure.AccessLevelLookup?.Name ?? string.Empty
        };
    }
}
