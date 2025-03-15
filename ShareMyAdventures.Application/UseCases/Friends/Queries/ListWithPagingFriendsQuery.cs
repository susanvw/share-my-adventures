using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;

namespace ShareMyAdventures.Application.UseCases.Friends.Queries;

public sealed record ListWithPagingFriendsQuery : IRequest<Result<PagedData<FriendView>?>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
} 

public sealed class ListWithPagingFriendsQueryHandler(ICurrentUser currentUserService, IParticipantRepository participantRepository) : IRequestHandler<ListWithPagingFriendsQuery, Result<PagedData<FriendView>?>>
{

    public async Task<Result<PagedData<FriendView>?>> Handle(ListWithPagingFriendsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var mapped = await participantRepository
            .ListFriendRequests(userId, InvitationStatusLookup.Pending)
            .Select(x => FriendView.MapFrom(x, x.ParticipantId))
            .ToPagedDataAsync(request.PageNumber, request.PageSize, cancellationToken: cancellationToken);

        return Result<PagedData<FriendView>?>.Success(mapped);

    }
}
