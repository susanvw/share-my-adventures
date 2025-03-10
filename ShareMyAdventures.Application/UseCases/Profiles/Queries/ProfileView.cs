using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Profiles.Queries;

public sealed record ProfileView
{
    public string Id { get; internal set; } = string.Empty;
    public string DisplayName { get; internal set; } = string.Empty;
    public string? Email { get; internal set; }
    public string Username { get; internal set; } = string.Empty;
    public byte[]? Photo { get; internal set; }
    public bool FollowMe { get; internal set; }
    public string? TrailColor { get; internal set; }

    internal static ProfileView MapFrom(Participant entity)
    {
        return new ProfileView
        {
            Id = entity.Id,
        };
    }
}
