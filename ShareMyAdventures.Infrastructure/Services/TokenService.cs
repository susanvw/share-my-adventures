using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShareMyAdventures.Application.Common.Guards;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShareMyAdventures.Infrastructure.Services;

public class TokenService(IOptions<JwtOptions> options) : ITokenService
{ 
 
    public (string JwtToken, DateTime JwtExpiryTime) GenerateAccessToken(Claim[] claims, int expireInMinutes)
    {
        var key = options.Value.SecretKey.ThrowIfNullOrWhiteSpace("Jwt Key is null");

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var expireTime = DateTime.Now.AddMinutes(expireInMinutes);
        var tokeOptions = new JwtSecurityToken(
            claims: claims,
            expires: expireTime,
            signingCredentials: signInCredentials
        );
        try
        {
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return (tokenString, expireTime);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }

        return (string.Empty, DateTime.Now);
    } 

    public Claim[] GenerateClaims(string userId, string username, string[] userRoles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userId),
            new(ClaimTypes.Email, username),
            new(ClaimTypes.NameIdentifier, userId)
        };

        foreach(var role in userRoles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return [.. claims];
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var key = options.Value.SecretKey.ThrowIfNullOrWhiteSpace("Jwt Key is null");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false, // for this part we do not care about the life time
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        catch (Exception ex)
        {
            _ = ex.Message;
            return null;
        }
    }
}
