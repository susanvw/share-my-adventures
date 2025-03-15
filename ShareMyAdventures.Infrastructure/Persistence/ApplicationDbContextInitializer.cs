using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Domain.Enums;

namespace ShareMyAdventures.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(RoleManager<IdentityRole> roleManager)
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

	public async Task SeedDefaultUserAsync()
    {
        foreach (var role in from role in UserRoleLookups.List
                             where _roleManager.Roles.All(r => r.Name != role.Name)
                             select role)
        {
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = role.Name
            });
        }
    }
}
