using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;

namespace ShareMyAdventures.Application.UseCases.Profiles.Commands;

public sealed class UploadAvatarCommand : IRequest<Unit>
{
    public string UserId { get; init; } = string.Empty;
    public string Photo { get; init; } = string.Empty;
}

internal class UploadAvatarCommandValidator : AbstractValidator<UploadAvatarCommand>
{
    internal UploadAvatarCommandValidator(ICurrentUser currentUserService)
    {
        string? currentUserId = currentUserService.UserId;
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty()
            .Must((userId) => ValidateCurrentUser(userId, currentUserId))
            .WithMessage("The User Id provided is not the current user that is logged in.");

        RuleFor(v => v.Photo).NotEmpty().NotNull();
        RuleFor(v => v.UserId).MinimumLength(5).MaximumLength(450);
    }

    private static bool ValidateCurrentUser(string userId, string? currentUserId)
    {
        if (currentUserId == null) return false;

        if (userId != currentUserId)
        {
            return false;
        }

        return true;
    }
}

public sealed class UploadAvatarCommandHandler(
    IParticipantRepository participantRepository,
    ICurrentUser currentUserService
    ) : IRequestHandler<UploadAvatarCommand, Unit>
{


    public async Task<Unit> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var validator = new UploadAvatarCommandValidator(currentUserService);
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var participant = await participantRepository.GetByIdAsync(request.UserId);
        participant = participant.ThrowIfNotFound(request.UserId);

        participant.SetProfilePhoto(request.Photo);

        await participantRepository.UpdateAsync(participant, cancellationToken);

        return Unit.Value;
    }
}
