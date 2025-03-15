using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;

namespace ShareMyAdventures.Application.UseCases.AdventureInvitations.Commands;

public sealed record RejectInvitationCommand : IRequest<Unit>
{
    public long AdventureId { get; init; }
    public long Id { get; init; }
}
internal sealed class RejectInvitationCommandValidator : AbstractValidator<RejectInvitationCommand>
{
    internal RejectInvitationCommandValidator()
    {
        RuleFor(v => v.Id).GreaterThan(0);
        RuleFor(v => v.AdventureId).GreaterThan(0);
    }
}

public sealed class RejectInvitationCommandHandler(
    IAdventureRepository adventureRepository

        ) : IRequestHandler<RejectInvitationCommand, Unit>
{
    public async Task<Unit> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
    {
        var validator = new RejectInvitationCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var adventure = await adventureRepository.GetByIdAsync(request.AdventureId, cancellationToken);
        adventure = adventure.ThrowIfNotFound(request.AdventureId);

        var invitation = adventure.GetInvitation(request.Id);
        invitation = invitation.ThrowIfNotFound(request.Id);

        invitation.UpdateStatus(InvitationStatusLookup.Rejected);

        adventure.UpdateInvitation(invitation);

        await adventureRepository.UpdateAsync(adventure, cancellationToken);

        return Unit.Value;
    }
}
