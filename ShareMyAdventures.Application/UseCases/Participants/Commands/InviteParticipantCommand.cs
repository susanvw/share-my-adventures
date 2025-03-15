using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.InvitationAggregate;

namespace ShareMyAdventures.Application.UseCases.Participants.Commands;

public sealed record InviteParticipantCommand : IRequest<Result<InviteParticipantResponse>>
{
    public long AdventureId { get; init; } 
    public string UserId { get; init; } = null!;
    public int AccessLevelLookupId { get; init; } 
}

public sealed class InviteParticipantResponse
{
    public long? Id { get; internal set; }
}

public class InviteParticipantCommandValidator : AbstractValidator<InviteParticipantCommand>
{
    public InviteParticipantCommandValidator()
    {
        RuleFor(v => v.UserId).MinimumLength(5).MaximumLength(450);
        RuleFor(v => v.AdventureId).GreaterThan(0);
        RuleFor(v => v.AccessLevelLookupId)
            .GreaterThan(0)
            .Must(CheckAccessLevelIdExist)
            .WithMessage("The Access Level Id does not exist.");
    }

    private static bool CheckAccessLevelIdExist(int id)
    {
        var entities = AccessLevelLookup.All;

        if (entities.Any(x => x.Id == id))
        {
            return true;
        }

        return false;
    }
}

public sealed class InviteParticipantCommandHandler(IAdventureRepository adventureRepository, IParticipantRepository participantRepository) : IRequestHandler<InviteParticipantCommand, Result<InviteParticipantResponse>>
{
    public async Task<Result<InviteParticipantResponse>> Handle(InviteParticipantCommand request,
        CancellationToken cancellationToken)
    {
        var user = await participantRepository.GetByIdAsync(request.UserId, cancellationToken);
        user = user.ThrowIfNotFound(request.UserId);

        var entity = await adventureRepository.GetByIdAsync(request.AdventureId, cancellationToken);
        entity = entity.ThrowIfNotFound(request.AdventureId);

        var accessLevel = AccessLevelLookup.All.FirstOrDefault(x => x.Id == request.AccessLevelLookupId);
        accessLevel = accessLevel.ThrowIfNotFound(request.AccessLevelLookupId);

        var invitation = entity.FindInvitationByEmail(user.Email!);

        if (invitation == null)
        {
            invitation = new AdventureInvitation(user.Email!, request.AdventureId, accessLevel, InvitationStatusLookup.Pending);
            entity.AddInvitation(invitation);
        }
        else if (invitation.InvitationStatusLookup == InvitationStatusLookup.Rejected)
        {
            invitation.UpdateStatus(InvitationStatusLookup.Pending);
        }

        invitation.UpdateAccessLevel(accessLevel);

        await adventureRepository.UpdateAsync(entity, cancellationToken);

        return Result<InviteParticipantResponse>.Success(new InviteParticipantResponse
        {
            Id = invitation.Id
        });
    }
}
