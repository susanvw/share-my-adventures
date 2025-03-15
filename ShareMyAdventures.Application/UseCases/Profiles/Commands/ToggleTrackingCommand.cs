using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Profiles.Commands;

public sealed record ToggleTrackingCommand : IRequest<Unit>
{
    public string UserId { get; init; } = string.Empty;
}

internal sealed class ToggleTrackingCommandValidator : AbstractValidator<ToggleTrackingCommand>
{
    internal ToggleTrackingCommandValidator(ICurrentUser currentUserService)
    {
        string? currentUserId = currentUserService.UserId;
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty()
            .Must((userId) => ValidateCurrentUser(userId, currentUserId))
            .WithMessage("The User Id provided is not the current user that is logged in.");
    }

    private bool ValidateCurrentUser(string userId, string? currentUserId)
    {
        if (currentUserId == null) return false;

        if (userId != currentUserId)
        {
            return false;
        }

        return true;
    }
}


public sealed class ToggleTrackingCommandHandler(IParticipantRepository participantRepository, ICurrentUser currentUserService) : IRequestHandler<ToggleTrackingCommand, Unit>
{
    public async Task<Unit> Handle(ToggleTrackingCommand request, CancellationToken cancellationToken)
    {
        var validator = new ToggleTrackingCommandValidator(currentUserService);
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var participant = await participantRepository.GetByIdAsync(request.UserId);
        participant = participant.ThrowIfNotFound(request.UserId);

        participant.ToggleFollowMe(!participant.FollowMe);

        await participantRepository.UpdateAsync(participant, cancellationToken);

        return Unit.Value;
    }

}
