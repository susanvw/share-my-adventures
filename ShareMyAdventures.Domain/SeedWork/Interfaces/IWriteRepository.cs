// Domain Project: SeedWork/Interfaces/IWriteRepository.cs
namespace ShareMyAdventures.Domain.SeedWork.Interfaces;

/// <summary>
/// Defines a generic write repository for performing CRUD operations and transactions
/// on aggregate roots within a domain-driven design context.
/// </summary>
/// <typeparam name="TModel">The type of the aggregate root entity, constrained to implement <see cref="IAggregateRoot{TId}"/>.</typeparam>
public interface IWriteRepository<TModel> where TModel : IAggregateRoot
{
    /// <summary>
    /// Asynchronously adds a new entity to the underlying data store without saving immediately.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// Changes are staged in the context and must be persisted using <see cref="SaveChangesAsync"/> or a transaction.
    /// </remarks>
    Task AddAsync(TModel entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously updates an existing entity in the underlying data store without saving immediately.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// Changes are staged in the context and must be persisted using <see cref="SaveChangesAsync"/> or a transaction.
    /// </remarks>
    Task UpdateAsync(TModel entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously deletes an existing entity from the underlying data store without saving immediately.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    /// <remarks>
    /// Changes are staged in the context and must be persisted using <see cref="SaveChangesAsync"/> or a transaction.
    /// </remarks>
    Task DeleteAsync(TModel entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously begins a new database transaction for batch operations.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a transaction is already in progress.</exception>
    /// <remarks>
    /// Use this method to group multiple operations (add, update, delete) into an atomic unit of work.
    /// </remarks>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously commits the current transaction, persisting all staged changes to the data store.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no transaction is in progress.</exception>
    /// <remarks>
    /// This method saves changes and commits the transaction. Use <see cref="RollbackTransactionAsync"/> to undo changes if needed.
    /// </remarks>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously rolls back the current transaction, discarding all staged changes.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no transaction is in progress.</exception>
    /// <remarks>
    /// Use this method to revert changes if an error occurs during a transactional operation.
    /// </remarks>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously saves all staged changes to the underlying data store.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// Call this method to persist changes outside of a transaction or after staging multiple operations.
    /// </remarks>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}