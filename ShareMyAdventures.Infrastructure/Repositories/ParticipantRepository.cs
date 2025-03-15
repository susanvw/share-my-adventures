using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Infrastructure.Persistence;

namespace ShareMyAdventures.Infrastructure.Repositories;

class ParticipantRepository(ApplicationDbContext context) : IParticipantRepository
{
    public Task<Participant?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return context.Participants.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }

    public async Task<bool> HasBeenInvitedAsync(Participant participant, Participant friend, CancellationToken cancellationToken  = default)
    {
        return await context
            .Users
            .AnyAsync(x =>
            x.Friends.Any(f => f.ParticipantFriendId == friend.Id && f.ParticipantId == participant.Id)
            ||
            x.Friends.Any(f => f.ParticipantFriendId == participant.Id && f.ParticipantId == friend.Id), cancellationToken: cancellationToken);
    }

    public IQueryable<FriendRequest> ListPendingFriendRequests(string participantId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(participantId);

        return context
            .Participants
            .Include(x => x.Friends)
                .ThenInclude(x => x.InvitationStatusLookup)
            .Include(x => x.Friends)
                .ThenInclude(x => x.Participant)
            .Where(x =>
                (x.Id == participantId || x.Friends.Any(f => f.ParticipantId == participantId)) &&
                x.Friends.Any(f => f.InvitationStatusLookup.Id == InvitationStatusLookup.Pending.Id))
            .SelectMany(x => x.Friends)
            .OrderBy(x => x.Participant.DisplayName); 
    }

    public IQueryable<Participant> Search(string filter)
    {
        return context.Participants
       .Where(x => x.DisplayName.Contains(filter) || x.Email!.Contains(filter))
       .OrderBy(x => x.DisplayName);
    }

    public async Task UpdateAsync(Participant entity, CancellationToken cancellationToken = default)
    {
        context.Participants.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}