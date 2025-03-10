using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Friends.Queries;

public sealed record FriendView
{
    public string Id { get; internal set; } = string.Empty;
    public string DisplayName { get; internal set; } = string.Empty;
    public byte[]? Photo { get; internal set; }

    internal static FriendView MapFrom(FriendRequest friend, string participantId)
    {
        var dto = new FriendView();

        // check if the current user is the friend or the participant
        var user = friend.ParticipantId == participantId ? friend.ParticipantFriend : friend.Participant;

        return dto;
    }
}
