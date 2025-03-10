using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Participants.Queries;

public sealed record GetParticipantQuery : IRequest<Result<ParticipantView?>>
{
    public string Id { get; init; } = string.Empty;
}

public class GetParticipantQueryValidator : AbstractValidator<GetParticipantQuery>
{
    public GetParticipantQueryValidator()
    {
        RuleFor(v => v.Id).MinimumLength(5).MaximumLength(450);
    }
}

public class GetParticipantQueryHandler(UserManager<Participant> userManager) : IRequestHandler<GetParticipantQuery, Result<ParticipantView?>>
{

    public async Task<Result<ParticipantView?>> Handle(GetParticipantQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetParticipantQueryValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var entity = await userManager.FindByIdAsync(request.Id);
        entity = entity.ThrowIfNotFound(request.Id);

        return Result<ParticipantView?>.Success(ParticipantView.MapFrom(entity));
    }
}

