using System.Security.Claims;

namespace ShareMyAdventures.Application.Common.Interfaces;

public interface ITokenService
{
    Claim[] GenerateClaims(string userId, string username, string[] userRoles);
    (string JwtToken, DateTime JwtExpiryTime) GenerateAccessToken(Claim[] claims, int expireInMinutes);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
