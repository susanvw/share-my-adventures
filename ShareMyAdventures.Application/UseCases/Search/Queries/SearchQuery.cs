using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;

namespace ShareMyAdventures.Application.UseCases.Search.Queries;

public sealed class SearchQuery : IRequest<Result<PagedData<SearchView>?>>
{
    public string Filter { get; init; } = string.Empty;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}


internal sealed class SearchQueryValidator : AbstractValidator<SearchQuery>
{
    internal SearchQueryValidator()
    {
        RuleFor(x => x.Filter).MinimumLength(5);
    }
}
public class SearchQueryHandler(IParticipantRepository participantRepository) : IRequestHandler<SearchQuery, Result<PagedData<SearchView>?>>
{

    public async Task<Result<PagedData<SearchView>?>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var validator = new SearchQueryValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var query = participantRepository.Search(request.Filter);
        var mapped = await query.Select(x => SearchView.MapFrom(x)).ToPagedDataAsync(request.PageNumber, request.PageSize, cancellationToken);


        return Result<PagedData<SearchView>?>.Success(mapped);
    }
}