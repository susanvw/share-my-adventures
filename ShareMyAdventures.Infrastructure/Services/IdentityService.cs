using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShareMyAdventures.Application.Common.Interfaces;
using ShareMyAdventures.Application.Common.Mappings;
using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.Common.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<Participant> _userManager;
    private readonly IReadRepository<Participant> _participantRepository;

    public IdentityService(UserManager<Participant> userManager, IReadRepository<Participant> participantRepository)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
    }

    public async Task<string?> GetUserNameAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.UserName;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName, CancellationToken cancellationToken = default)
    {
        // Assuming you have a policy-based authorization setup (e.g., via AuthorizationHandler)
        var user = await _userManager.FindByIdAsync(userId);
        return user != null; // Placeholder; implement policy check as needed
    }

    public async Task<Participant?> GetParticipantAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<PagedData<FriendRequest>> GetPendingFriendRequestsAsync(string participantId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var friendRequests = await _participantRepository
            .Include(x => x.Friends)
                .ThenInclude<FriendRequest>(x => x.InvitationStatusLookup)
            .Include(x => x.Friends)
                .ThenInclude<FriendRequest>(x => x.Participant)
            .FindByCustomFilter(x =>
                (x.Id == participantId || x.Friends.Any(f => f.ParticipantId == participantId)) &&
                x.Friends.Any(f => f.InvitationStatusLookup.Id == InvitationStatusLookup.Pending.Id))
            .SelectMany(x => x.Friends)
            .OrderBy(x => x.Participant.DisplayName)
            .ToPagedDataAsync(pageNumber, pageSize);

        return friendRequests;
    }
}