using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Profiles.Commands;

public sealed record UpdateProfileCommand : IRequest<Unit>
{
    public string UserId { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? TrailColor { get; init; }
    public bool FollowMe { get; init; } = false;
}

internal sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    internal UpdateProfileCommandValidator(ICurrentUser currentUserService)
    {
        string? currentUserId = currentUserService.UserId;
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty()
            .Must((userId) => ValidateCurrentUser(userId, currentUserId))
            .WithMessage("The User Id provided is not the current user that is logged in.");

        RuleFor(v => v.DisplayName).MinimumLength(5).MaximumLength(32);
        RuleFor(v => v.UserId).MinimumLength(5).MaximumLength(450);
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

public sealed class UpdateProfileCommandHandler(UserManager<Participant> userManager, ICurrentUser currentUserService) : IRequestHandler<UpdateProfileCommand, Unit>
{

    public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateProfileCommandValidator(currentUserService);
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var participant = await userManager.FindByIdAsync(request.UserId);
        participant = participant.ThrowIfNotFound(request.UserId);

        participant.FollowMe = request.FollowMe;
        participant.DisplayName = request.DisplayName;
        participant.TrailColor = request.TrailColor;

        await userManager.UpdateAsync(participant);

        return Unit.Value;
    }
}
