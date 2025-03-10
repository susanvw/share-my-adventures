using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Application.UseCases.Participants.Queries;

public sealed record ListWithPagingParticipantsQuery : IRequest<Result<PagedData<ParticipantView>?>>
{
    public long AdventureId { get; init; } = 0;
    public string? Filter { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
} 

public sealed class ListParticipantsQueryValidator : AbstractValidator<ListWithPagingParticipantsQuery>
{
    public ListParticipantsQueryValidator()
    {
        RuleFor(x => x.AdventureId).GreaterThan(0);
    }
}

public class ListParticipantsQueryHandler(
    IReadableRepository<Adventure> adventureReadableRepository,
    IWriteRepository<Adventure> adventureRepository
    ) : IRequestHandler<ListWithPagingParticipantsQuery, Result<PagedData<ParticipantView>?>>
{

    public async Task<Result<PagedData<ParticipantView>?>> Handle(ListWithPagingParticipantsQuery request, CancellationToken cancellationToken)
    {
        var validator = new ListParticipantsQueryValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var query = await adventureReadableRepository
            .Include(x => x.Participants)
            .FindOneByCustomFilter(x =>
                x.Id == request.AdventureId &&
                (
                 request.Filter == null ||
                 x.Participants.Any
                    (p =>
                        p.Participant.DisplayName.Contains(request.Filter.Trim()) ||
                        p.Participant.UserName!.Contains(request.Filter.Trim())
                    )
                )
            )
            .SelectMany(x => x.Participants)
            .Select(x => x.Participant)
            .OrderBy(x => x.DisplayName)
            .Select(x => ParticipantView.MapFrom(x))
            .ToPagedDataAsync(request.PageNumber, request.PageSize);

        return Result<PagedData<ParticipantView>?>.Success(query);
    }
}

