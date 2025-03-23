
using Common.Adapter.Email;
using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.Enums;

namespace ShareMyAdventures.Application.UseCases.Accounts.Command;

public sealed record RegisterAccountCommand : IRequest<Result<string?>>
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string CallbackUrl { get; init; } = null!;
}

internal sealed class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
{
    internal RegisterAccountCommandValidator()
    {
        RuleFor(v => v.Username).MinimumLength(5).MaximumLength(256).EmailAddress();
        RuleFor(v => v.Password).MinimumLength(5);
        RuleFor(v => v.CallbackUrl).MinimumLength(5);
    }
}

public sealed class RegisterAccountCommandHandler(
    UserManager<Participant> identityService,
    IEmailSender emailSender,
    IRoleStore<IdentityRole> roleManager) :
    IRequestHandler<RegisterAccountCommand, Result<string?>>
{
    public async Task<Result<string?>> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        var validator = new RegisterAccountCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = new Participant(request.Username, request.Username);

        var createResult = await identityService.CreateAsync(user, request.Password);

        if (createResult.Succeeded)
        {
            var token = await identityService.GenerateEmailConfirmationTokenAsync(user);
            var content = EmailExtensions.GetConfirmAccountEmail(user, token, request.CallbackUrl);

            var sender = await emailSender.SendHtmlAsync(user.Email!, "Confirm Email.", content, cancellationToken);

            if (!sender.IsSuccessStatusCode)
            {
                return Result<string?>.Failure("Could not send the email.");
            }
            
            // check if role exists
            var role = await roleManager.FindByNameAsync(UserRoleLookups.User.Name, cancellationToken);
            if (role == null)
            {
                role = new IdentityRole(UserRoleLookups.User.Name)
                {
                    NormalizedName = UserRoleLookups.User.Name
                };
                var results = await roleManager.CreateAsync(role, cancellationToken);
            }
            
            var addUserToRoleResult = await identityService.AddToRoleAsync(user, UserRoleLookups.User.Name);

            return addUserToRoleResult.Succeeded ? Result<string?>.Success(user.Id) : Result<string?>.Failure([.. addUserToRoleResult.Errors.Select(x => x.Description)]);

        }
        else
        {
            return Result<string?>.Failure([..createResult.Errors.Select(x => x.Description)]);
        }

    }
}

