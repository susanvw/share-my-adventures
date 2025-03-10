using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Application.UseCases.Adventures.Queries;

public sealed record ListWithPagingAdventuresByStatusIdQuery : IRequest<Result<PagedData<AdventureView>?>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int StatusLookupId { get; init; } = 0;
}
 

public sealed class ListWithPagingAdventuresByStatusIdQueryHandler(
    IReadableRepository<Adventure> adventureReadableRepository,
    ICurrentUser currentUserService
        ) : IRequestHandler<ListWithPagingAdventuresByStatusIdQuery, Result<PagedData<AdventureView>?>>
{

    public async Task<Result<PagedData<AdventureView>?>> Handle(ListWithPagingAdventuresByStatusIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var query = adventureReadableRepository
            .Include(x => x.Participants)
            .ThenInclude<ParticipantAdventure>(x => x.AccessLevelLookup)
            .Include(x => x.TypeLookup)
            .Include(x => x.StatusLookup)
            .Include(x => x.MeetupLocationLookup)
            .Include(x => x.DestinationLocationLookup)
            .FindOneByCustomFilter(x =>
                x.Participants.Any(p => p.ParticipantId == userId) &&
                x.StatusLookupId == request.StatusLookupId)
            .Select(x => AdventureView.MapFrom(x));

        var paged = await query.ToPagedDataAsync(request.PageNumber, request.PageSize);


        return Result<PagedData<AdventureView>?>.Success(paged);

    }
}
