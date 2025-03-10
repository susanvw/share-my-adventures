using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Authentication.Commands;

public sealed record AuthenticateCommand : IRequest<Result<AuthView?>>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

internal sealed class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
{
    internal AuthenticateCommandValidator()
    {
        RuleFor(v => v.Username).MinimumLength(5).MaximumLength(256).EmailAddress();
        RuleFor(v => v.Password).MinimumLength(5);
    }
}

public sealed class AuthenticateCommandHandler( 
    UserManager<Participant> userManager,
    SignInManager<Participant> signInManager,
    ITokenService tokenService
    ) : IRequestHandler<AuthenticateCommand, Result<AuthView?>>
{

    public async Task<Result<AuthView?>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var validator = new AuthenticateCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await userManager.FindByEmailAsync(request.Username);
        if (user == null)
        {
            return Result<AuthView?>.Failure("User could not be signed in.");
        }

        var signInResult = await signInManager.PasswordSignInAsync(user, request.Password, true, false);

        if (signInResult.IsLockedOut)
        {

            return Result<AuthView?>.Failure("The user has been locked out. Please contact your administrator.");
        }
        else if (signInResult.IsNotAllowed)
        {
            return Result<AuthView?>.Failure("The user is not allowed to login. Please check if you have confirmed your account.");
        }
        else if (signInResult.RequiresTwoFactor)
        {
            return Result<AuthView?>.Failure("The user requires 2 factor authentication. Please contact your administrator.");
        }
        else if (!user.EmailConfirmed)
        {
            return Result<AuthView?>.Failure("Please confirm your Email Account.");
        }
        else if (signInResult.Succeeded)
        {
            // update user details
            var refreshToken = tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await userManager.UpdateAsync(user);

            var userRoles = await userManager.GetRolesAsync(user);
            var claims = tokenService.GenerateClaims(user.Id, user.Email!, [..userRoles]);
            await userManager.AddClaimsAsync(user, claims);


            var (JwtToken, JwtExpiryTime) = tokenService.GenerateAccessToken(claims, 15);

            return Result<AuthView?>.Success(new AuthView
            {
                JwtToken = JwtToken,
                JwtExpiryTime = JwtExpiryTime,
                RefreshToken = refreshToken,
                RoleNames = [.. userRoles],
                UserId = user.Id
            });
        }
        else
        {
            return Result<AuthView?>.Failure("User could not be signed in.");
        }
    }
}
