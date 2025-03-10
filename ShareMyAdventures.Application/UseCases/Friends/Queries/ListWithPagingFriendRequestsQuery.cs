using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Friends.Queries;

public sealed record ListWithPagingFriendRequestsQuery : IRequest<Result<PagedData<FriendRequestView>?>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

}

public class ListWithPagingFriendRequestsQueryHandler(
    ICurrentUser currentUserService,
    IReadableRepository<FriendRequest> friendRepository,
    UserManager<Participant> userManager
    ) : IRequestHandler<ListWithPagingFriendRequestsQuery, Result<PagedData<FriendRequestView>?>>
{

    public async Task<Result<PagedData<FriendRequestView>?>> Handle(ListWithPagingFriendRequestsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrWhiteSpace("Current User");

        var participant = await userManager.FindByIdAsync(userId);
        participant = participant.ThrowIfNotFound(userId);

        var mapped = await friendRepository
            .Include(x => x.ParticipantFriend)
            .Include(x => x.InvitationStatusLookup)
            .Include(X => X.Participant)
            .FindOneByCustomFilter(x =>
            (x.ParticipantId == participant.Id || x.ParticipantFriendId == participant.Id) &&
             x.InvitationStatusLookupId == Domain.Enums.InvitationStatusLookups.Pending.Id)           
            .OrderBy(x => x.ParticipantFriend.DisplayName)
            .Select(x => FriendRequestView.MapFrom(x, participant.Id))
            .ToPagedDataAsync(request.PageNumber, request.PageSize);


        return Result<PagedData<FriendRequestView> ?>.Success(mapped);

    }
}