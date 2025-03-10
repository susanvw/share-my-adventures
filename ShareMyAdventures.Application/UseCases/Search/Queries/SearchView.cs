using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Search.Queries;

public sealed record SearchView
{
    public string Id { get; internal set; } = string.Empty;
    public string DisplayName { get; internal set; } = string.Empty;
    public string Email { get; internal set; } = string.Empty;
    public byte[]? Photo { get; internal set; }

    internal static SearchView MapFrom(FriendRequest entity, string userId)
    {
        // check if the current user is the friend or the participant
        var user = entity.Participant.Id == userId ? entity.ParticipantFriend : entity.Participant;

        return new SearchView
        {
            DisplayName = user.DisplayName,
        };
    }
}
