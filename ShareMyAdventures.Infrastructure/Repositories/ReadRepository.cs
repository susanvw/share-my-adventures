// Infrastructure Project: Repositories/ReadableRepository.cs
using ShareMyAdventures.Domain.SeedWork;
using ShareMyAdventures.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace ShareMyAdventures.Infrastructure.Repositories;

/// <summary>
/// A generic read-only repository implementation for querying aggregate roots using Entity Framework Core.
/// </summary>
/// <typeparam name="TModel">The type of the model that must inherit from <see cref="BaseEntity"/>.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="ReadRepository{TModel}"/> class.
/// </remarks>
/// <param name="dbContext">The Entity Framework DbContext instance to use for data operations.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="dbContext"/> is null.</exception>
public sealed class ReadRepository<TModel>(ApplicationDbContext dbContext) : IReadRepository<TModel>
    where TModel : BaseEntity, IAggregateRoot
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
 
    public IQueryable<TModel> GetQueryable()
    {
        return dbContext.Set<TModel>().AsNoTracking();
    }
    /// <inheritdoc />
    public async Task<IEnumerable<TModel>> FindByCustomFilterAsync(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return await _dbContext.Set<TModel>()
            .Where(expression)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TModel?> FindOneByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TModel>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TModel?> FindOneByCustomFilterAsync(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return await _dbContext.Set<TModel>()
            .AsNoTracking()
            .FirstOrDefaultAsync(expression, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TModel>> FindManyByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default)
    {
        if (ids == null || !ids.Any())
            return [];

        return await _dbContext.Set<TModel>()
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);
    }
    /// <inheritdoc />

    public IQueryable<TModel> FindByCustomFilter(Expression<Func<TModel, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return _dbContext.Set<TModel>()
            .Where(expression)
        .AsNoTracking();
    }
}