// Infrastructure Project: Repositories/WriteRepository.cs
using Microsoft.EntityFrameworkCore.Storage;
using ShareMyAdventures.Domain.SeedWork;
using ShareMyAdventures.Infrastructure.Persistence;

namespace ShareMyAdventures.Infrastructure.Repositories;

/// <summary>
/// A generic write repository implementation for performing CRUD operations and transactions
/// on aggregate roots using Entity Framework Core.
/// </summary>
/// <typeparam name="TContext">The type of the Entity Framework DbContext.</typeparam>
/// <typeparam name="TModel">The type of the aggregate root entity, constrained to implement <see cref="IAggregateRoot{TId}"/>.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="WriteRepository{TContext, TModel, TId}"/> class.
/// </remarks>
/// <param name="dbContext">The Entity Framework DbContext instance to use for data operations.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="dbContext"/> is null.</exception>
public sealed class WriteRepository<TModel>(ApplicationDbContext dbContext) : IWriteRepository<TModel>, IDisposable
    where TModel : class, IAggregateRoot
{
    private IDbContextTransaction?_transaction;
    private bool _disposed;

    /// <inheritdoc />
    public async Task AddAsync(TModel entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await dbContext.Set<TModel>().AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TModel entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        dbContext.Set<TModel>().Update(entity);
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(TModel entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        dbContext.Set<TModel>().Remove(entity);
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
            throw new InvalidOperationException("A transaction is already in progress.");
        _transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken)
            ;
    }

    /// <inheritdoc />
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction in progress.");
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    /// <inheritdoc />
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction in progress.");
        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously disposes of the current transaction, if any, and resets the transaction state.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Releases all resources used by the <see cref="WriteRepository{TContext, TModel, TId}"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="WriteRepository{TContext, TModel, TId}"/>
    /// and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    private void Dispose(bool disposing)
    {
        if (_disposed) 
            return;

        if (disposing)
        {
            _transaction?.Dispose();
        }

        _disposed = true;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="WriteRepository{TContext, TModel, TId}"/> class.
    /// </summary>
    ~WriteRepository()
    {
        Dispose(false);
    }
}