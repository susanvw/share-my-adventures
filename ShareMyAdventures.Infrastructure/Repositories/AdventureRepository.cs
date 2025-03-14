using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Infrastructure.Persistence;

namespace ShareMyAdventures.Infrastructure.Repositories;

public class AdventureRepository(ApplicationDbContext context) : IAdventureRepository
{
    public async Task<long?> AddAsync(Adventure entity, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public IQueryable<Adventure?> FindForParticipant(long id, string participantId)
    {
        return context.Adventures
        .Include(x => x.Participants)
        .ThenInclude(x => x.AccessLevelLookup)
        .Include(x => x.TypeLookup)
        .Include(x => x.StatusLookup)
        .Include(x => x.MeetupLocationLookup)
        .Include(x => x.DestinationLocationLookup)
        .Where(x =>
                x.Id == id &&
                x.Participants.Any(a => a.ParticipantId == participantId));
    }

    public async Task<Adventure?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await context.Adventures.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(Adventure entity, CancellationToken cancellationToken = default)
    {
        context.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Adventure entity, CancellationToken cancellationToken = default)
    {
        context.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
