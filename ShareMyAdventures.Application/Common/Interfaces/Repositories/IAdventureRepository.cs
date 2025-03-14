using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Application.Common.Interfaces.Repositories
{
    public interface IAdventureRepository
    {
        Task<long?> AddAsync(Adventure entity, CancellationToken cancellationToken = default);
        IQueryable<Adventure?> FindForParticipant(long id, string participantId);
        Task<Adventure?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task RemoveAsync(Adventure entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(Adventure entity, CancellationToken cancellationToken = default);
    }
}