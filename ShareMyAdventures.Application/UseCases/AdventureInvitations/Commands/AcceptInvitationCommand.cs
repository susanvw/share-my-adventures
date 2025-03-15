using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.AdventureInvitations.Commands;

public sealed record AcceptInvitationCommand : IRequest<Unit>
{
    public long AdventureId { get; init; } = 0;
    public string Email { get; init; } = string.Empty;
}

internal sealed class AcceptInvitationCommandValidator : AbstractValidator<AcceptInvitationCommand>
{
    public AcceptInvitationCommandValidator()
    {
        RuleFor(x => x.AdventureId)
            .GreaterThan(0);

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
    }
}


public sealed class AcceptInvitationCommandHandler(
    UserManager<Participant> userManager,
    IReadRepository<Adventure> adventureReadableRepository,
    IWriteRepository<Adventure> adventureRepository
        )
    : IRequestHandler<AcceptInvitationCommand, Unit>
{
    public async Task<Unit> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var entity = await
            adventureReadableRepository
            .FindOneByIdAsync(request.AdventureId, cancellationToken);
        entity = entity.ThrowIfNotFound(request.AdventureId);

        var invitation = entity.FindInvitationByEmail(request.Email);
        invitation = invitation.ThrowIfNotFound(request.Email);

        invitation.UpdateStatus(InvitationStatusLookup.Accepted);

        // check if user exist. If not create user
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            user = new Participant(request.Email, request.Email);
            await userManager.CreateAsync(user, "Password@1");
        }

        var adventureParticipant = new ParticipantAdventure(user.Id, invitation.AccessLevelLookup);
        entity.AddParticipant(adventureParticipant!);

        await adventureRepository.UpdateAsync(entity, cancellationToken);
        await adventureRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
