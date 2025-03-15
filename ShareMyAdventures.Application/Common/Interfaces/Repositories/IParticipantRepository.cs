using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.Common.Interfaces.Repositories;

public interface IParticipantRepository
{
    Task<Participant?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> HasBeenInvitedAsync(Participant participant, Participant friend, CancellationToken cancellationToken = default);
    IQueryable<FriendRequest> ListFriendRequests(string participantId, InvitationStatusLookup? invitationStatusLookup = null);
    IQueryable<Participant> Search(string filter);
    Task UpdateAsync(Participant entity, CancellationToken cancellationToken = default);
}