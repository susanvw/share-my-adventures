
using Common.Adapter.Email;
using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Password.Commands;

public record SendPasswordReminderCommand : IRequest<Result<string?>>
{
    public string Username { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
}

internal sealed class SendPasswordReminderCommandValidator : AbstractValidator<SendPasswordReminderCommand>
{
    internal SendPasswordReminderCommandValidator()
    {
        RuleFor(v => v.Username).NotEmpty().NotNull().MinimumLength(5);
    }
}


public class AuthenticateCommandHandler(
    IEmailSender emailSender,
    UserManager<Participant> identityService
        ) : IRequestHandler<SendPasswordReminderCommand, Result<string?>>
{

    public async Task<Result<string?>> Handle(SendPasswordReminderCommand request, CancellationToken cancellationToken)
    {
        var validator = new SendPasswordReminderCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var participant = await identityService.FindByEmailAsync(request.Username);
        participant = participant.ThrowIfNotFound(request.Username);

        var token = await identityService.GeneratePasswordResetTokenAsync(participant);
        var content = EmailExtensions.GetResetPasswordEmail(participant.Email!, token, request.Url);

        var sender = await emailSender.SendHtmlAsync(participant.Email!, "Reset Password.", content, cancellationToken: cancellationToken);

        if (!sender.IsSuccessStatusCode)
        {
            return Result<string?>.Failure(["Could not send the email."]);
        }

        return Result<string?>.Success(participant.Id);
    }
}
