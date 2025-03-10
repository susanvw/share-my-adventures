
using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Password.Commands;

public sealed record ResetPasswordCommand : IRequest<Unit>
{
    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string Token { get; init; } = string.Empty;
}

internal sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{

    internal ResetPasswordCommandValidator()
    {
        RuleFor(v => v.Username).MinimumLength(4).MaximumLength(256);
        RuleFor(v => v.Password).NotEmpty().MinimumLength(4);
        RuleFor(v => v.Token).NotEmpty();
    }
}
public sealed class ResetPasswordCommandHandler(UserManager<Participant> identityService) : IRequestHandler<ResetPasswordCommand, Unit>
{ 

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var validator = new ResetPasswordCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var participant = await identityService.FindByEmailAsync(request.Username);
        participant = participant.ThrowIfNotFound(request.Username);

        await identityService.ResetPasswordAsync(participant, request.Token, request.Password);

        return Unit.Value;

    }
}