using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;

namespace ShareMyAdventures.Application.UseCases.Friends.Queries;

/// <summary>
/// Represents a query to retrieve a paged list of pending friend requests for the current user.
/// </summary>
public sealed record ListWithPagingFriendRequestsQuery : IRequest<Result<PagedData<FriendRequestView>?>>
{
    /// <summary>
    /// Gets the page number (1-based) for pagination.
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; init; } = 10;
}

/// <summary>
/// Handles the <see cref="ListWithPagingFriendRequestsQuery"/> to fetch pending friend requests with paging.
/// </summary>
public class ListWithPagingFriendRequestsQueryHandler(ICurrentUser currentUserService, IParticipantRepository participantRepository) : IRequestHandler<ListWithPagingFriendRequestsQuery, Result<PagedData<FriendRequestView>?>>
{
    public async Task<Result<PagedData<FriendRequestView>?>> Handle(ListWithPagingFriendRequestsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrWhiteSpace("Current user ID is missing");

        var friendRequests = await participantRepository
            .ListFriendRequests(userId)
            .Select(x => FriendRequestView.MapFrom(x, userId))
            .ToPagedDataAsync(request.PageNumber, request.PageSize, cancellationToken);

        return Result<PagedData<FriendRequestView>?>.Success(friendRequests);
    }
}