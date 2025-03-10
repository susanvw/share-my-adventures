using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Enums;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Application.UseCases.AdventureInvitations.Commands;

public sealed record RejectInvitationCommand : IRequest<Unit>
{
    public long Id { get; init; }
}
internal sealed class RejectInvitationCommandValidator : AbstractValidator<RejectInvitationCommand>
{
    internal RejectInvitationCommandValidator()
    {
        RuleFor(v => v.Id).GreaterThan(0);
    }
}

public sealed class RejectInvitationCommandHandler(
    IReadableRepository<Adventure> adventureReadableRepository,
    IWriteRepository<Adventure> adventureRepository
        ) : IRequestHandler<RejectInvitationCommand, Unit>
{
    public async Task<Unit> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
    {
        var validator = new RejectInvitationCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var adventure = await adventureReadableRepository.FindOneByIdAsync(request.Id, cancellationToken);
        adventure = adventure.ThrowIfNotFound(request.Id);

        adventure.StatusLookupId = InvitationStatusLookups.Rejected.Id;
        await adventureRepository.UpdateAsync(adventure, cancellationToken);
        await adventureRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
