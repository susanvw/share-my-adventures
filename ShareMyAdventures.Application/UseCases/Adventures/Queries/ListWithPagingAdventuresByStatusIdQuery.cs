using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;

namespace ShareMyAdventures.Application.UseCases.Adventures.Queries;

public sealed record ListWithPagingAdventuresByStatusIdQuery : IRequest<Result<PagedData<AdventureView>?>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int StatusLookupId { get; init; }
}
 

public sealed class ListWithPagingAdventuresByStatusIdQueryHandler(
    IAdventureRepository adventureRepository,
    ICurrentUser currentUserService
        ) : IRequestHandler<ListWithPagingAdventuresByStatusIdQuery, Result<PagedData<AdventureView>?>>
{

    public async Task<Result<PagedData<AdventureView>?>> Handle(ListWithPagingAdventuresByStatusIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var paged = await adventureRepository
            .FindByStatus(request.StatusLookupId, userId)
            .Select(x => AdventureView.MapFrom(x))
            .ToPagedDataAsync(request.PageNumber, request.PageSize, cancellationToken); 

        return Result<PagedData<AdventureView>?>.Success(paged);

    }
}
