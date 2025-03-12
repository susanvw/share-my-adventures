using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

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
public class ListWithPagingFriendRequestsQueryHandler : IRequestHandler<ListWithPagingFriendRequestsQuery, Result<PagedData<FriendRequestView>?>>
{
    private readonly ICurrentUser _currentUserService;
    private readonly IIdentityService _identityService;

    public ListWithPagingFriendRequestsQueryHandler(ICurrentUser currentUserService, IIdentityService identityService)
    {
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
    }

    public async Task<Result<PagedData<FriendRequestView>?>> Handle(ListWithPagingFriendRequestsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId.ThrowIfNullOrWhiteSpace("Current user ID is missing");

        var friendRequests = await _identityService.GetPendingFriendRequestsAsync(userId, request.PageNumber, request.PageSize, cancellationToken);
        var mapped = friendRequests.MapItems(x => FriendRequestView.MapFrom(x, userId));

        return Result<PagedData<FriendRequestView>?>.Success(mapped);
    }
}