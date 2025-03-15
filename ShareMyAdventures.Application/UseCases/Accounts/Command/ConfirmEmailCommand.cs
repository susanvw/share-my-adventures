using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ValidationException = ShareMyAdventures.Application.Common.Exceptions.ValidationException;

namespace ShareMyAdventures.Application.UseCases.Accounts.Command;

public sealed record ConfirmEmailCommand : IRequest<Unit>
{
    public string Email { get; init; } = null!;
    public string Token { get; init; } = null!;
}

internal class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    internal ConfirmEmailCommandValidator()
    {
        RuleFor(v => v.Email).MinimumLength(5).MaximumLength(256).EmailAddress();
        RuleFor(v => v.Token).MinimumLength(5);
    }
}


public sealed class ConfirmEmailCommandHandler(UserManager<Participant> userManager) : IRequestHandler<ConfirmEmailCommand, Unit>
{

    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var validator = new ConfirmEmailCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await userManager.FindByEmailAsync(request.Email);
        user = user.ThrowIfNotFound(request.Email);

        var decodedToken = request.Token.Decode();

        var result = await userManager.ConfirmEmailAsync(user!, decodedToken);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => new ValidationFailure(x.Code, x.Description)).ToArray();
            throw new ValidationException(errors);
        }

        return new Unit();
    }
}
