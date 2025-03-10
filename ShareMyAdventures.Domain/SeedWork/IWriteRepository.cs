namespace ShareMyAdventures.Domain.SeedWork;

/// <summary>
/// Defines a generic write repository for performing CRUD operations and transaction management
/// on aggregate roots within a domain-driven design context.
/// </summary>
/// <typeparam name="TModel">The type of the aggregate root entity, which must implement <see cref="IAggregateRoot"/>.</typeparam>
public interface IWriteRepository<in TModel> where TModel : IAggregateRoot
{
    /// <summary>
    /// Asynchronously stages a new entity for addition to the data store.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// Changes are staged and must be persisted using <see cref="SaveChangesAsync"/> or a transaction.
    /// </remarks>
    Task AddAsync(TModel entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously stages an existing entity for update in the data store.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// Changes are staged and must be persisted using <see cref="SaveChangesAsync"/> or a transaction.
    /// </remarks>
    Task UpdateAsync(TModel entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously stages an existing entity for deletion from the data store.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// Changes are staged and must be persisted using <see cref="SaveChangesAsync"/> or a transaction.
    /// </remarks>
    Task DeleteAsync(TModel entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously begins a new database transaction for atomic operations.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a transaction is already active.</exception>
    /// <remarks>
    /// Use this to group multiple operations into a single atomic unit of work.
    /// </remarks>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously commits the current transaction, persisting all staged changes.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no transaction is active.</exception>
    /// <remarks>
    /// Commits all staged changes within the transaction to the data store.
    /// </remarks>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously rolls back the current transaction, discarding all staged changes.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no transaction is active.</exception>
    /// <remarks>
    /// Reverts all staged changes if an error occurs during the transaction.
    /// </remarks>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously persists all staged changes to the data store.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// Persists changes outside a transaction or after staging multiple operations.
    /// </remarks>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}