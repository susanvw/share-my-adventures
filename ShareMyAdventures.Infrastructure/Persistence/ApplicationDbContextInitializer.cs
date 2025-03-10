using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Domain.Enums;

namespace ShareMyAdventures.Infrastructure.Persistence;

public class ApplicationDbContextInitializer(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

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

    public void SeedEnums()
    {
        foreach (var access in from Domain.Enums.AccessLevelLookups access in Domain.Enums.AccessLevelLookups.List
                               where !_context.AccessLevelLookups.Any(x => x.Name == access.Name)
                               select access)
        {
            _context.AccessLevelLookups.Add(new Domain.Entities.AccessLevelLookup
            {
                Name = access.Name,
                Id = access.Id
            });
        }

        foreach (var invitation in from Domain.Enums.InvitationStatusLookups invitation in Domain.Enums.InvitationStatusLookups.List
                               where !_context.InvitationStatusLookups.Any(x => x.Name == invitation.Name)
                               select invitation)
        {
            _context.InvitationStatusLookups.Add(new Domain.Entities.InvitationStatusLookup
            {
                Name = invitation.Name,
                Id = invitation.Id
            });
        }

        foreach (var status in from Domain.Enums.StatusLookups status in Domain.Enums.StatusLookups.List
                                   where !_context.StatusLookups.Any(x => x.Name == status.Name)
                                   select status)
        {
            _context.StatusLookups.Add(new Domain.Entities.StatusLookup
            {
                Name = status.Name,
                Id = status.Id
            });
        }

        foreach (var type in from Domain.Enums.TypeLookups status in Domain.Enums.TypeLookups.List
                             where !_context.TypeLookups.Any(x => x.Name == status.Name)
                             select status)
        {
            _context.TypeLookups.Add(new Domain.Entities.TypeLookup
            {
                Name = type.Name,
                Id = type.Id
            });
        }

        _context.SaveChanges();
    }
}
