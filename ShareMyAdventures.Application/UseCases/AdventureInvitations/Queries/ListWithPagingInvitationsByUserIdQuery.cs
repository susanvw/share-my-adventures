﻿using Microsoft.EntityFrameworkCore;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.InvitationAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.AdventureInvitations.Queries;

public sealed record ListWithPagingInvitationsByUserIdQuery : IRequest<Result<PagedData<InvitationView>?>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public sealed class GetInvitationsQueryHandler(
    IAdventureRepository adventureRepository,
    ICurrentUser currentUserService,
    IParticipantRepository participantRepository
        ) : IRequestHandler<ListWithPagingInvitationsByUserIdQuery, Result<PagedData<InvitationView>?>>
{

    public async Task<Result<PagedData<InvitationView>?>> Handle(ListWithPagingInvitationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var user = await participantRepository.GetByIdAsync(userId, cancellationToken);
        user = user.ThrowIfNotFound(userId);

        var query = adventureRepository
            .FindByStatus(InvitationStatusLookup.Pending.Id, userId)
            .SelectMany(x => x.Invitations)
        .Select(x => InvitationView.MapFrom(x, user.Email!));

        var mapped = await query.ToPagedDataAsync(request.PageNumber, request.PageSize, cancellationToken: cancellationToken);

        return Result<PagedData<InvitationView>?>.Success(mapped);
    }
}
