using Microsoft.EntityFrameworkCore;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.Positions.Queries;

public sealed record GetLatestForAdventureQuery : IRequest<Result<IReadOnlyList<PositionView>?>>
{
    public long AdventureId { get; init; } = 0;
    public DateTime FromDate { get; init; } = DateTime.UtcNow;
} 

internal sealed class GetLatestForAdventureQueryValidator : AbstractValidator<GetLatestForAdventureQuery>
{
    internal GetLatestForAdventureQueryValidator()
    {
        RuleFor(x => x.AdventureId).NotEmpty().NotNull();
    }
}

public class ListForAdventureQueryHandler(
    IAdventureRepository adventureRepository
        ) : IRequestHandler<GetLatestForAdventureQuery, Result<IReadOnlyList<PositionView>?>>
{

    public async Task<Result<IReadOnlyList<PositionView>?>> Handle(GetLatestForAdventureQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetLatestForAdventureQueryValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var positions = await adventureRepository
            .ListPositions(request.AdventureId, request.FromDate)
            .Select(x => PositionView.MapFrom(x))
            .ToListAsync(cancellationToken); 

        return Result<IReadOnlyList<PositionView>?>.Success(positions);
    }
}
