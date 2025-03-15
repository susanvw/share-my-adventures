using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.UseCases.Authentication.Commands;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.GoogleSignIn;

public sealed record GoogleSignInCommand : IRequest<Result<AuthView?>>
{
    public string Email { get; init; } = null!;
    public string? Name { get; init; }
    public string? FamilyName { get; init; }
    public string? GivenName { get; init; }
    public string Id { get; init; } = null!;
    public string? ProfileUrl { get; init; }
} 

public sealed class GoogleSignInCommandHandler(
    UserManager<Participant> userManager,
    ITokenService tokenService
    ) : IRequestHandler<GoogleSignInCommand, Result<AuthView?>>
{

    public async Task<Result<AuthView?>> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
    {
        var existing = await userManager.FindByEmailAsync(request.Email);

        var displayName = request.Email;

        if (!string.IsNullOrEmpty(request.GivenName))
        {
            displayName = request.GivenName;
        }
        else if (!string.IsNullOrEmpty(request.Name))
        {
            displayName = request.Name;
        }

        if (existing == null)
        {
            // register new user
            var applicationUser = new Participant
            {
                Email = request.Email,
                UserName = request.Email
            };

            var createResult = await userManager.CreateAsync(applicationUser);

            if (createResult.Succeeded)
            {
                existing = await userManager.FindByEmailAsync(request.Email);
            }
            else
            {
                return Result<AuthView?>.Failure([.. createResult.Errors.Select(x => x.Description)]);
            }
        }
        else
        {
            existing.UpdateProfile(displayName);

            if (request.ProfileUrl != null)
            {
                existing.SetProfilePhoto(request.ProfileUrl);
            }
        }

        if (existing == null)
        {
            return Result<AuthView?>.Failure(["User could not be found."]);
        }


        // update user details
        var refreshToken = tokenService.GenerateRefreshToken();
        existing.RefreshToken = refreshToken;
        existing.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await userManager.UpdateAsync(existing);

        var userRoles = await userManager.GetRolesAsync(existing);
        var claims = tokenService.GenerateClaims(existing.Id, existing.Email!, [.. userRoles]);
        await userManager.AddClaimsAsync(existing, claims);

        var (JwtToken, JwtExpiryTime) = tokenService.GenerateAccessToken(claims, 15);

        return Result<AuthView?>.Success(new AuthView
        {
            JwtToken = JwtToken,
            JwtExpiryTime = JwtExpiryTime,
            RefreshToken = refreshToken,
            RoleNames = [.. userRoles],
            UserId = existing.Id
        });
    }
}