namespace ShareMyAdventures.Infrastructure;

public sealed record JwtOptions
{
    public const string Options = "JwtSettings";
    public string SecretKey { get; set; } = string.Empty;
    public double ExpiresInMinutes { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
