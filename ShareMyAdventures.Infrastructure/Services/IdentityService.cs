using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Infrastructure.Services;

public class IdentityService(
    UserManager<Participant> userManager
    ) : IIdentityService
{
    public async Task<string?> GetUserNameAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user?.UserName;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user != null && await userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName, CancellationToken cancellationToken = default)
    {
        // Assuming you have a policy-based authorization setup (e.g., via AuthorizationHandler)
        var user = await userManager.FindByIdAsync(userId);
        return user != null; // Placeholder; implement policy check as needed
    }

    /// <inheritdoc/>
    public async Task<Participant?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await userManager.FindByIdAsync(userId);
    }
}