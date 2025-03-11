using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Infrastructure.Identity;

public sealed class ApplicationUserManager(
    IUserStore<Participant> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<Participant> passwordHasher,
    IEnumerable<IUserValidator<Participant>> userValidators,
    IEnumerable<IPasswordValidator<Participant>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<ApplicationUserManager> logger) 
    : UserManager<Participant>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    , IApplicationUserManager
{
    public Task FindFriendAsync(string userId, long id)
    {

    }
}
