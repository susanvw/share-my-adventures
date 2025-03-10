using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Accounts.Command;

public sealed record DeleteAccountCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
}

internal sealed class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    internal DeleteAccountCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().NotNull();
    }
}

public sealed class DeleteProfileCommandHandler(UserManager<Participant> userManager) : IRequestHandler<DeleteAccountCommand, Unit>
{

    public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteAccountCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await userManager.FindByIdAsync(request.UserId);
        user = user.ThrowIfNotFound(request.UserId);

        await userManager.DeleteAsync(user);

        return Unit.Value;
    }
}

