using Microsoft.EntityFrameworkCore;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

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
    IReadableRepository<Position> positionRepository,
    IReadableRepository<Adventure> adventureReadableRepository
        ) : IRequestHandler<GetLatestForAdventureQuery, Result<IReadOnlyList<PositionView>?>>
{

    public async Task<Result<IReadOnlyList<PositionView>?>> Handle(GetLatestForAdventureQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetLatestForAdventureQueryValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var adventure = await adventureReadableRepository.FindOneByIdAsync(request.AdventureId, cancellationToken);
        adventure = adventure.ThrowIfNotFound(request.AdventureId);

        var query = await positionRepository
         .FindOneByCustomFilter(x =>
         x.TimeStamp != null &&
         DateTime.Parse(x.TimeStamp) >= adventure.StartDate && DateTime.Parse(x.TimeStamp) <= adventure.EndDate)
         .Select(x => PositionView.MapFrom(x))
         .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<PositionView>?>.Success(query);
    }
}
