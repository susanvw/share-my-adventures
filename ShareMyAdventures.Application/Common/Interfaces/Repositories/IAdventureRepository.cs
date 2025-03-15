using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.Common.Interfaces.Repositories;

public interface IAdventureRepository
{
    Task<long?> AddAsync(Adventure entity, CancellationToken cancellationToken = default);
    IQueryable<Adventure> FindForParticipant(long id, string participantId);
    Task<Adventure?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> HasActiveAdventuresAsync(long id, string userId, CancellationToken cancellationToken = default);
    IAdventureRepository IncludeInvitations();
    Task RemoveAsync(Adventure entity, CancellationToken cancellationToken = default);
    IQueryable<Adventure> SearchParticipants(long adventureId, string? filter);
    Task UpdateAsync(Adventure entity, CancellationToken cancellationToken = default);
    IQueryable<Adventure> FindByStatus(int statusLookupId, string userId); // New method to find by status
    IQueryable<Position> ListPositions(long adventureId, DateTime fromDate); // List positions for an adventure
}
