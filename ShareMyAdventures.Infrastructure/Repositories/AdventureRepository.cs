// ShareMyAdventures.Infrastructure/Repositories/AdventureRepository.cs
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Infrastructure.Persistence;

namespace ShareMyAdventures.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Adventure entities, providing data access operations.
/// Implements the IAdventureRepository interface for the Adventure aggregate.
/// </summary>
public class AdventureRepository : IAdventureRepository
{
    private readonly ApplicationDbContext _context;
    private IQueryable<Adventure> _query; // Holds the query state for fluent chaining

    /// <summary>
    /// Initializes a new instance of the AdventureRepository.
    /// </summary>
    /// <param name="context">The database context for data access.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
    public AdventureRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _query = _context.Adventures.AsQueryable(); // Initialize default query
    }

    /// <summary>
    /// Applies all navigation property includes to an Adventure query.
    /// Centralizes the eager loading of related entities for consistency.
    /// </summary>
    /// <param name="query">The base query to extend with includes.</param>
    /// <returns>An IQueryable with all navigation properties included.</returns>
    private static IQueryable<Adventure> IncludeAllNavigationProperties(IQueryable<Adventure> query)
    {
        return query
            .Include(x => x.Participants)
                .ThenInclude(x => x.AccessLevelLookup)
            .Include(x => x.Participants)
                .ThenInclude(x => x.Participant)
            .Include(x => x.Invitations)
            .Include(x => x.TypeLookup)
            .Include(x => x.StatusLookup)
            .Include(x => x.MeetupLocationLookup)
            .Include(x => x.DestinationLocationLookup);
    }

    /// <summary>
    /// Configures the query to include the Invitations navigation property.
    /// </summary>
    /// <returns>The repository instance for fluent chaining.</returns>
    public IAdventureRepository IncludeInvitations()
    {
        _query = _query.Include(x => x.Invitations); // Modify the query state
        return this; // Return self for chaining
    }

    /// <summary>
    /// Adds a new Adventure entity to the database and returns its ID.
    /// </summary>
    /// <param name="entity">The Adventure entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The ID of the newly added Adventure, or null if not set.</returns>
    public async Task<long?> AddAsync(Adventure entity, CancellationToken cancellationToken = default)
    {
        await _context.Adventures.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    /// <summary>
    /// Retrieves an Adventure by its ID, applying any configured includes (e.g., Invitations).
    /// Resets the query state after execution.
    /// </summary>
    /// <param name="id">The ID of the Adventure to retrieve.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The Adventure entity if found; otherwise, null.</returns>
    public async Task<Adventure?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        // Execute the query with any configured includes
        var result = await _query.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        // Reset the query state to the default after execution
        _query = _context.Adventures.AsQueryable();
        return result;
    }

    /// <summary>
    /// Finds Adventures where a specific participant is involved, including all navigation properties.
    /// </summary>
    /// <param name="id">The ID of the Adventure to find.</param>
    /// <param name="participantId">The ID of the participant to filter by.</param>
    /// <returns>An IQueryable of matching Adventures with all navigation properties included.</returns>
    public IQueryable<Adventure> FindForParticipant(long id, string participantId)
    {
        return IncludeAllNavigationProperties(_context.Adventures)
            .Where(x => x.Id == id && x.Participants.Any(a => a.ParticipantId == participantId));
    }

    /// <summary>
    /// Checks if an Adventure is active (InProgress) and includes a specific participant.
    /// </summary>
    /// <param name="id">The ID of the Adventure to check.</param>
    /// <param name="userId">The ID of the participant (user) to verify.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>True if the Adventure is active and includes the participant; otherwise, false.</returns>
    public Task<bool> HasActiveAdventuresAsync(long id, string userId, CancellationToken cancellationToken = default)
    {
        return _context.Adventures
            .AnyAsync(x => x.Id == id &&
                          x.StatusLookup.Id == StatusLookup.InProgress.Id &&
                          x.Participants.Any(p => p.ParticipantId == userId),
                      cancellationToken);
    }

    /// <summary>
    /// Searches for Adventures by participant details, filtering by a search term.
    /// </summary>
    /// <param name="adventureId">The ID of the Adventure to search within.</param>
    /// <param name="filter">Optional search term to filter participants by DisplayName or UserName.</param>
    /// <returns>An IQueryable of Adventures with Participants included, filtered by the search term.</returns>
    public IQueryable<Adventure> SearchParticipants(long adventureId, string? filter)
    {
        return _context.Adventures
            .Include(x => x.Participants)
                .ThenInclude(p => p.Participant)
            .Where(x => x.Id == adventureId &&
                       (filter == null ||
                        x.Participants.Any(p => p.Participant.DisplayName.Contains(filter.Trim()) ||
                                               p.Participant.UserName.Contains(filter.Trim()))));
    }

    /// <summary>
    /// Updates an existing Adventure entity in the database.
    /// </summary>
    /// <param name="entity">The Adventure entity with updated data.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous update operation.</returns>
    public async Task UpdateAsync(Adventure entity, CancellationToken cancellationToken = default)
    {
        _context.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Removes an Adventure entity from the database.
    /// </summary>
    /// <param name="entity">The Adventure entity to remove.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous remove operation.</returns>
    public async Task RemoveAsync(Adventure entity, CancellationToken cancellationToken = default)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Finds Adventures with a specific status, based on the StatusLookup ID.
    /// </summary>
    /// <param name="statusLookupId">The ID of the status to filter by (e.g., StatusLookup.InProgress.Id).</param>
    /// <param name="userId"></param>
    /// <returns>An IQueryable of Adventures matching the specified status.</returns>
    public IQueryable<Adventure> FindByStatus(int statusLookupId, string userId)
    {
        return _context.Adventures
            .Where(x =>
                x.StatusLookup.Id == statusLookupId &&
                x.Participants.Any(p => p.ParticipantId == userId)
            ); // Filter by StatusLookup ID
    }

    /// <summary>
    /// Lists all Positions for Participants in a specific Adventure, 
    /// filtered by the Adventure's StartDate and EndDate.
    /// </summary>
    /// <param name="adventureId">The ID of the Adventure to query positions for.</param>
    /// <returns>An IQueryable of Positions for the Adventure's participants within the date range.</returns>
    public IQueryable<Position> ListPositions(long adventureId, DateTime fromDate)
    {
        // Fetch the adventure to get its date range
        var adventure = _context.Adventures
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == adventureId)
            ?? throw new InvalidOperationException($"Adventure with ID {adventureId} not found.");

        // Get all participant IDs for this adventure
        var participantIds = _context.ParticipantAdventures
            .Where(pa => pa.AdventureId == adventureId)
            .Select(pa => pa.ParticipantId)
            .Distinct();

        // Query Positions for those participants within the adventure's date range
        return _context.Positions
            .Where(p => participantIds.Contains(p.ParticipantId) &&
                        p.TimeStamp != null && // Guard against null timestamps
                        DateTime.Parse(p.TimeStamp) >= fromDate &&
                        DateTime.Parse(p.TimeStamp) <= adventure.EndDate);
    }
}