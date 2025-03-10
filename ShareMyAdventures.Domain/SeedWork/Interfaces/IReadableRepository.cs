// Domain Project: SeedWork/Interfaces/IReadableRepository.cs (or Application Project: Interfaces/IReadableRepository.cs) 
using System.Linq.Expressions;

namespace ShareMyAdventures.Domain.SeedWork.Interfaces;

/// <summary>
/// Interface for read-only repository operations for a model.
/// Provides methods to query and retrieve data from a data store in a CQRS architecture.
/// </summary>
/// <typeparam name="TModel">The type of the model that must implement the <see cref="BaseEntity"/> interface.</typeparam>
public interface IReadableRepository<TModel> where TModel : BaseEntity
{
    /// <summary>
    /// Specifies a navigation property to include in the query.
    /// </summary>
    /// <param name="includeExpression">The navigation property to include.</param>
    /// <returns>The repository instance for further chaining.</returns>
    IReadableRepository<TModel> Include(Expression<Func<TModel, object>> includeExpression);

    /// <summary>
    /// Specifies a related navigation property to include after a previous Include.
    /// </summary>
    /// <typeparam name="TPreviousProperty">The type of the previous navigation property.</typeparam>
    /// <param name="thenIncludeExpression">The related navigation property to include.</param>
    /// <returns>The repository instance for further chaining.</returns>
    IReadableRepository<TModel> ThenInclude<TPreviousProperty>(
        Expression<Func<TPreviousProperty, object>> thenIncludeExpression);

    /// <summary>
    /// Asynchronously retrieves all records of the specified model type.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a collection of all model records.</returns>
    IQueryable<TModel> FindOneByCustomFilter(Expression<Func<TModel, bool>> expression); 

    /// <summary>
    /// Asynchronously retrieves records that match the specified custom filter.
    /// </summary>
    /// <param name="expression">An expression that defines the filter criteria.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation, yielding a collection of model records that match the filter.</returns>
    Task<IEnumerable<TModel>> FindByCustomFilterAsync(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a single record by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the model to retrieve.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation, with the model record if found, or <c>null</c> if not.</returns>
    Task<TModel?> FindOneByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a single record that matches the specified custom filter.
    /// </summary>
    /// <param name="expression">An expression that defines the filter criteria.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation, with the model record if found, or <c>null</c> if not.</returns>
    Task<TModel?> FindOneByCustomFilterAsync(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves multiple records by their identifiers.
    /// </summary>
    /// <param name="ids">A collection of identifiers of the models to retrieve.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous operation, yielding a collection of model records matching the identifiers.</returns>
    Task<IEnumerable<TModel>> FindManyByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default); 
}