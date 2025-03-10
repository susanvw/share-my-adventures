using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Interfaces;
using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Tokens.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<Result<RefreshTokenResponse>>
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshTokenResponse 
{
    public string? AccessToken { get; internal set; }
    public string? RefreshToken { get; internal set; }
    public DateTime? RefreshTokenExpires { get; internal set; }


}
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x).NotNull().NotEmpty();
    }
}

public class RefreshTokenCommandHandler(ITokenService tokenService, UserManager<Participant> userManager)
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly UserManager<Participant> _userManager = userManager;

    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validator = new RefreshTokenCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        string accessToken = request.AccessToken;
        string refreshToken = request.RefreshToken;
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

        if (principal?.Claims == null)
            return Result<RefreshTokenResponse>.Failure(["User token could not be refreshed."]);

        var userId = principal.Identities.FirstOrDefault()?.Name;

        if (userId == null)
            return Result<RefreshTokenResponse>.Failure(["User token could not be refreshed."]);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return Result<RefreshTokenResponse>.Failure(["User token could not be refreshed."]);

        // update user details
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        var userRoles = await _userManager.GetRolesAsync(user);
        var claims = _tokenService.GenerateClaims(user.Id, user.UserName!, [.. userRoles]);
        await _userManager.AddClaimsAsync(user, claims);
        var (JwtToken, JwtExpiryTime) = _tokenService.GenerateAccessToken(claims, 15);

        return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse
        {
            AccessToken = JwtToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpires = JwtExpiryTime
        });
    }
}
