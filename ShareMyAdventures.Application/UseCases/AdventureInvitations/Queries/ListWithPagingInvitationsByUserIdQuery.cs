using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Application.UseCases.AdventureInvitations.Queries;

public sealed record ListWithPagingInvitationsByUserIdQuery : IRequest<Result<PagedData<InvitationView>?>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public sealed class GetInvitationsQueryHandler(
    IReadableRepository<Adventure> adventureReadableRepository,
    ICurrentUser currentUserService,
    UserManager<Participant> userManager
        ) : IRequestHandler<ListWithPagingInvitationsByUserIdQuery, Result<PagedData<InvitationView>?>>
{

    public async Task<Result<PagedData<InvitationView>?>> Handle(ListWithPagingInvitationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var user = await userManager.FindByIdAsync(userId);
        user = user.ThrowIfNotFound(userId);

        var query = adventureReadableRepository
        .Include(x => x.StatusLookup)
        .Include(x => x.TypeLookup)
        .Include(x => x.Participants)
            .ThenInclude<ParticipantAdventure>(x => x.AccessLevelLookup)
        .FindOneByCustomFilter(x =>
            x.Participants.Any(p => p.Participant.Email == user.Email!) &&
            x.StatusLookupId == Domain.Enums.InvitationStatusLookups.Pending.Id
        )
        .OrderBy(x => x.StartDate)
        .SelectMany(x => x.Invitations)
        .Select(x => InvitationView.MapFrom(x, user.Email!));


        var mapped = await query.ToPagedDataAsync(request.PageNumber, request.PageSize);

        return Result<PagedData<InvitationView>?>.Success(mapped);
    }
}
