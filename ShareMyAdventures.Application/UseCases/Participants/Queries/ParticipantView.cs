using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Participants.Queries;

public sealed record ParticipantView
{
    public string Id { get; internal set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string DisplayName { get; internal set; } = string.Empty;
    public string? Photo { get; internal set; }
    public bool FollowMe { get; internal set; }
    public string? TrailColor { get; internal set; }
  

    internal static ParticipantView MapFrom( Participant user)
    { 
        return new ParticipantView
        {
            DisplayName = user.DisplayName,
            FollowMe = user.FollowMe,
            Id = user.Id,
            Photo = user.Photo,
            TrailColor = user.TrailColor,
            UserName = user.UserName!
        };
    }
}
