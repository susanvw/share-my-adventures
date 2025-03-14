using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Search.Queries;

public sealed record SearchView
{
    public required string Id { get;  set; } 
    public required string DisplayName { get;  set; }
    public required string Email { get;  set; } 
    public byte[]? Photo { get; set; }

    internal static readonly Func<Participant, SearchView> MapFrom = (entity) =>
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new SearchView
        {
            Id = entity.Id,
            DisplayName = entity.DisplayName,
            Email = entity.Email!,
            //Photo = entity.Photo
        };
    };
}
