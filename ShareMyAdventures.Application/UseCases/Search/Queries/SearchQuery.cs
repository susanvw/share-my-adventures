using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

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
public class SearchQueryHandler(
    IReadableRepository<FriendRequest> friendRepository,
    ICurrentUser currentUserService) : IRequestHandler<SearchQuery, Result<PagedData<SearchView>?>>
{

    public async Task<Result<PagedData<SearchView>?>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var validator = new SearchQueryValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var userId = currentUserService.UserId.ThrowIfNotFound("current user");

        var mapped = await friendRepository
            .Include(x => x.ParticipantFriend)
            .Include(x => x.Participant)
            .FindOneByCustomFilter(x =>
            (x.ParticipantFriend.DisplayName.Contains(request.Filter) ||
            x.ParticipantFriend.Email!.Contains(request.Filter) ||
            x.Participant.DisplayName.Contains(request.Filter) ||
            x.Participant.Email!.Contains(request.Filter)) &&
            (x.Participant.Id == userId || x.ParticipantFriend.Id == userId))
            .OrderBy(x => x.ParticipantFriend.DisplayName) 
            .Select(x => SearchView.MapFrom(x, userId))
            .ToPagedDataAsync(request.PageNumber, request.PageSize);

        return Result<PagedData<SearchView>?>.Success(mapped);
    }
}