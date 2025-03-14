using ShareMyAdventures.Application.UseCases.Search.Queries;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.Common.Interfaces.Repositories;

public interface IParticipantRepository
{
    Task<bool> HasBeenInvitedAsync(Participant participant, Participant friend, CancellationToken cancellationToken);
    IQueryable<FriendRequest> ListPendingFriendRequests(string participantId);
    IQueryable<Participant> Search(string filter);
}