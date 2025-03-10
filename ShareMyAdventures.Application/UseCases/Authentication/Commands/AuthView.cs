namespace ShareMyAdventures.Application.UseCases.Authentication.Commands;

public sealed record AuthView
{
    public string UserId { get; internal set; } = string.Empty;
    public string JwtToken { get; internal set; } = string.Empty;
    public string RefreshToken { get; internal set; } = string.Empty;
    public List<string> RoleNames { get; internal set; } = [];
    public DateTime JwtExpiryTime { get; internal set; }
}
