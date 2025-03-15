using Microsoft.EntityFrameworkCore;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;

namespace ShareMyAdventures.Application.UseCases.Adventures.Queries;

public sealed record GetAdventureByIdQuery : IRequest<Result<AdventureView?>>
{
    public long Id { get; init; } 
}

public sealed class GetAdventureQueryHandler(
    IAdventureRepository adventureRepository,
    ICurrentUser currentUserService)
    : IRequestHandler<GetAdventureByIdQuery, Result<AdventureView?>>
{

    public async Task<Result<AdventureView?>> Handle(GetAdventureByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");


        var entity = await adventureRepository
            .FindForParticipant(request.Id, userId)
            .Select(x => AdventureView.MapFrom(x))
            .FirstOrDefaultAsync(cancellationToken);

        entity = entity.ThrowIfNotFound(request.Id);

        return Result<AdventureView?>.Success(entity);
    }
}
