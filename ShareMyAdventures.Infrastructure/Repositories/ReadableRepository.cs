// Infrastructure Project: Repositories/ReadableRepository.cs
using ShareMyAdventures.Domain.SeedWork;
using ShareMyAdventures.Domain.SeedWork.Interfaces;
using ShareMyAdventures.Infrastructure.Persistence;
using System.Linq.Expressions;
using System.Threading;

namespace ShareMyAdventures.Infrastructure.Repositories;

/// <summary>
/// A generic read-only repository implementation for querying aggregate roots using Entity Framework Core.
/// </summary>
/// <typeparam name="TModel">The type of the model that must inherit from <see cref="BaseEntity"/>.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="ReadableRepository{TModel}"/> class.
/// </remarks>
/// <param name="dbContext">The Entity Framework DbContext instance to use for data operations.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="dbContext"/> is null.</exception>
public sealed class ReadableRepository<TModel>(ApplicationDbContext dbContext) : IReadableRepository<TModel>
    where TModel : BaseEntity
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly List<(Expression IncludeExpression, List<Expression> ThenIncludeExpressions)> _includes = [];

    /// <inheritdoc />
    public async Task<IEnumerable<TModel>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TModel>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
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

    public IQueryable<TModel> FindAll()
    {
        throw new NotImplementedException();
    }

    public IReadableRepository<TModel> Include(Expression<Func<TModel, object>> includeExpression)
    {
        _includes.Add((includeExpression, new List<Expression>()));
        return this;
    }

    public Task UpdateAsync(object entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IReadableRepository<TModel> ThenInclude<TPreviousProperty>(
            Expression<Func<TPreviousProperty, object>> thenIncludeExpression)
    {
        if (_includes.Count == 0)
        {
            throw new InvalidOperationException("ThenInclude must follow an Include call.");
        }

        // Add ThenInclude to the last Include's list
        var lastInclude = _includes[^1];
        lastInclude.ThenIncludeExpressions.Add(thenIncludeExpression);
        _includes[^1] = lastInclude; // Update the tuple
        return this;
    }

    public IQueryable<TModel> FindOneByCustomFilter(Expression<Func<TModel, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return _dbContext.Set<TModel>()
            .Where(expression)
        .AsNoTracking();
    }
}