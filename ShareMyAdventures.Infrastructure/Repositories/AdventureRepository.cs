using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
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

    public IQueryable<Adventure> FindForId(long id, string userId)
    {
        throw new NotImplementedException();
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

    public Task<bool> HasActiveAdventuresAsync(int id, string userId, CancellationToken cancellationToken = default)
    {
        return context
            .Adventures
            .AnyAsync(x =>
                x.Id == id &&
                x.StatusLookup == StatusLookup.InProgress &&
                x.Participants.Any(x => x.ParticipantId == userId)
             ,
            cancellationToken);
    }

    public IQueryable<Adventure> ListByStatusId(int statusLookupId, string userId)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Position> ListPositions(long adventureId, DateTime fromDate)
    {
        var adventure = context.Adventures.Find([adventureId]);
        adventure = adventure.ThrowIfNotFound(adventureId);

        return context
            .Positions
            .Where(x => DateTime.Parse(x.TimeStamp) >= fromDate
             && DateTime.Parse(x.TimeStamp) <= adventure.EndDate
             && x.Id == adventureId);
    }

    public async Task RemoveAsync(Adventure entity, CancellationToken cancellationToken = default)
    {
        context.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<Adventure> SearchParticipants(long adventureId, string? filter)
    {
        return context
        .Adventures
        .Include(x => x.Participants)
        .Where(x =>
            x.Id == adventureId &&
            (
             filter == null ||
             x.Participants.Any
                (p =>
                    p.Participant.DisplayName.Contains(filter.Trim()) ||
                    p.Participant.UserName!.Contains(filter.Trim())
                )
            )
        );
    }

    public async Task UpdateAsync(Adventure entity, CancellationToken cancellationToken = default)
    {
        context.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
