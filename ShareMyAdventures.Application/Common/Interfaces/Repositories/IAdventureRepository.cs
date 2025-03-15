using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.Common.Interfaces.Repositories;

public interface IAdventureRepository
{
    Task<long?> AddAsync(Adventure entity, CancellationToken cancellationToken = default);
    IQueryable<Adventure> FindForId(long id, string userId);
    IQueryable<Adventure?> FindForParticipant(long id, string participantId);
    Task<Adventure?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> HasActiveAdventuresAsync(int id, string userId, CancellationToken cancellationToken = default);
    IQueryable<Adventure> ListByStatusId(int statusLookupId, string userId);
    IQueryable<Position> ListPositions(long adventureId, DateTime fromDate);
    Task RemoveAsync(Adventure entity, CancellationToken cancellationToken = default);
    IQueryable<Adventure> SearchParticipants(long adventureId, string? filter);
    Task UpdateAsync(Adventure entity, CancellationToken cancellationToken = default);
}