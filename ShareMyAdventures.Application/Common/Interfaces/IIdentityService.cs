namespace ShareMyAdventures.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId, CancellationToken cancellationToken = default);

    Task<bool> IsInRoleAsync(string userId, string role, CancellationToken cancellationToken = default);

    Task<bool> AuthorizeAsync(string userId, string policyName, CancellationToken cancellationToken = default);
}
