using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.Friends.Queries;

public sealed record ListWithPagingFriendsQuery : IRequest<Result<PagedData<FriendView>?>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
} 

public sealed class ListWithPagingFriendsQueryHandler(
    ICurrentUser currentUserService,
    IReadRepository<FriendRequest> friendRepository) : IRequestHandler<ListWithPagingFriendsQuery, Result<PagedData<FriendView>?>>
{

    public async Task<Result<PagedData<FriendView>?>> Handle(ListWithPagingFriendsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("User Id");

        var mapped = await friendRepository
            .Include(x => x.ParticipantFriend)
            .Include(x => x.Participant)
            .FindOneByCustomFilter(x => x.ParticipantFriend.Id == userId || x.Participant.Id == userId)
            .OrderBy(x => x.Participant.DisplayName)
            .Select(x => FriendView.MapFrom(x, x.ParticipantId))
            .ToPagedDataAsync(request.PageNumber, request.PageSize);

        return Result<PagedData<FriendView>?>.Success(mapped);

    }
}
