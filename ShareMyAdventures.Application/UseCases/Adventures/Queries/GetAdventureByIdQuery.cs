using Microsoft.EntityFrameworkCore;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.Adventures.Queries;

public sealed record GetAdventureByIdQuery : IRequest<Result<AdventureView?>>
{
    public long Id { get; init; } 
}

public sealed class GetAdventureQueryHandler(
    IReadRepository<Adventure> adventureReadableRepository,
    ICurrentUser currentUserService)
    : IRequestHandler<GetAdventureByIdQuery, Result<AdventureView?>>
{

    public async Task<Result<AdventureView?>> Handle(GetAdventureByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");


        var entity = await adventureReadableRepository
            .Include(x => x.MeetupLocationLookup)
            .Include(x => x.Participants)
            .Include(x => x.StatusLookup)
            .Include(x => x.TypeLookup)
            .Include(x => x.DestinationLocationLookup)
            .FindOneByCustomFilter(x => x.Id == request.Id && x.Participants.Any(p => p.ParticipantId == userId))
            .Select(x => AdventureView.MapFrom(x))
            .FirstOrDefaultAsync(cancellationToken);

        entity = entity.ThrowIfNotFound(request.Id);

        return Result<AdventureView?>.Success(entity);
    }
}
