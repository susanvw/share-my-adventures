using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Friends.Queries;

public sealed record FriendRequestView
{
    public long Id { get; internal set; }
    public string DisplayName { get; internal set; } = string.Empty;
    public byte[]? Photo { get; internal set; }
    public int InvitationStatusLookupId { get; internal set; } = 0;
    public string? InvitationStatusLookupIdName { get; internal set; }
    public bool CanAccept { get; internal set; } = false;

    internal static FriendRequestView MapFrom(FriendRequest entity, string currentParticipantId)
    {
        var entityToMap = entity.ParticipantId == currentParticipantId ? entity.ParticipantFriend : entity.Participant;


        return new FriendRequestView
        {

            InvitationStatusLookupIdName = entity.InvitationStatusLookup?.Name,
            CanAccept = entity.ParticipantFriendId == currentParticipantId

        };
    }
}
