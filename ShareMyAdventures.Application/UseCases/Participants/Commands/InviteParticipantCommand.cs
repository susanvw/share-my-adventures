using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.Enums;
using ShareMyAdventures.Domain.SeedWork;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Application.UseCases.Participants.Commands;

public sealed record InviteParticipantCommand : IRequest<Result<InviteParticipantResponse>>
{
    public long AdventureId { get; init; } = 0;
    public string UserId { get; init; } = string.Empty;
    public int AccessLevelLookupId { get; init; } = 0;
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
        var entities = Enumeration.GetAll<AccessLevelLookups>();

        if (entities.Any(x => x.Id == id))
        {
            return true;
        }

        return false;
    }
}

public sealed class InviteParticipantCommandHandler(
    IReadableRepository<Adventure> adventureReadableRepository,
    IWriteRepository<Adventure> adventureRepository,
    UserManager<Participant> identityService
    ) : IRequestHandler<InviteParticipantCommand, Result<InviteParticipantResponse>>
{
    public async Task<Result<InviteParticipantResponse>> Handle(InviteParticipantCommand request,
        CancellationToken cancellationToken)
    {
        var user = await identityService.FindByIdAsync(request.UserId);
        user = user.ThrowIfNotFound(request.UserId);

        var entity = await adventureReadableRepository
            .Include(x => x.Invitations)
            .FindOneByIdAsync(request.AdventureId, cancellationToken);

        entity = entity.ThrowIfNotFound(request.AdventureId);

        var invitation = entity.FindByEmail(user.Email!);

        if (invitation == null)
        {
            invitation = new AdventureInvitation
            {
                AdventureId = request.AdventureId,
                Email = user.Email!,
                AccessLevelLookupId = request.AccessLevelLookupId,
                InvitationStatusLookupId = Domain.Enums.InvitationStatusLookups.Pending.Id
            };

            entity.Invitations.Add(invitation);
        }
        else if (invitation.InvitationStatusLookupId == Domain.Enums.InvitationStatusLookups.Rejected.Id)
        {
            invitation.InvitationStatusLookupId = Domain.Enums.InvitationStatusLookups.Pending.Id;
            invitation.AccessLevelLookupId = request.AccessLevelLookupId;
        }
        else if (invitation.InvitationStatusLookupId == Domain.Enums.InvitationStatusLookups.Accepted.Id)
        {
            invitation.AccessLevelLookupId = request.AccessLevelLookupId;
        }

        await adventureRepository.UpdateAsync(entity, cancellationToken);
        await adventureRepository.SaveChangesAsync(cancellationToken);

        return Result<InviteParticipantResponse>.Success(new InviteParticipantResponse
        {
            Id = invitation.Id
        });
    }
}
