﻿using System.Linq.Expressions;

namespace ShareMyAdventures.Domain.SeedWork;

/// <summary>
/// Defines a read-only repository interface for querying domain models.
/// Supports basic retrieval operations and navigation property inclusion, aligning with CQRS principles
/// by separating read concerns from write operations.
/// </summary>
/// <typeparam name="TModel">The type of the domain model, which must inherit from <see cref="IAggregateRoot"/>.</typeparam>
public interface IReadRepository<TModel> 
    where TModel : BaseEntity, IAggregateRoot
{
    /// <summary>
    /// Specifies a navigation property to include in the query results.
    /// Useful for eagerly loading related data in an ORM context.
    /// </summary>
    /// <param name="includeExpression">An expression specifying the navigation property to include.</param>
    /// <returns>The current repository instance for method chaining.</returns>
    IReadRepository<TModel> Include(Expression<Func<TModel, object>> includeExpression);

    /// <summary>
    /// Specifies a related navigation property to include after a previous <see cref="Include"/> call.
    /// Enables deeper eager loading of nested relationships.
    /// </summary>
    /// <typeparam name="TPreviousProperty">The type of the previously included navigation property.</typeparam>
    /// <param name="thenIncludeExpression">An expression specifying the related navigation property to include.</param>
    /// <returns>The current repository instance for method chaining.</returns>
    IReadRepository<TModel> ThenInclude<TPreviousProperty>(
        Expression<Func<TPreviousProperty, object>> thenIncludeExpression);

    /// <summary>
    /// Retrieves a queryable collection of records matching the specified filter.
    /// Allows for further LINQ composition before execution.
    /// </summary>
    /// <param name="expression">An expression defining the filter criteria.</param>
    /// <returns>An <see cref="IQueryable{TModel}"/> representing the filtered records.</returns>
    IQueryable<TModel> FindByCustomFilter(Expression<Func<TModel, bool>> expression);

    /// <summary>
    /// Asynchronously retrieves all records matching the specified filter.
    /// </summary>
    /// <param name="expression">An expression defining the filter criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task yielding a collection of records that match the filter.</returns>
    Task<IEnumerable<TModel>> FindByCustomFilterAsync(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves a single record by its unique identifier.
    /// </summary>
    /// <param name="id">The identifier of the record to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task yielding the record if found; otherwise, <c>null</c>.</returns>
    Task<TModel?> FindOneByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the first record matching the specified filter.
    /// </summary>
    /// <param name="expression">An expression defining the filter criteria.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task yielding the first matching record if found; otherwise, <c>null</c>.</returns>
    Task<TModel?> FindOneByCustomFilterAsync(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves multiple records by their unique identifiers.
    /// </summary>
    /// <param name="ids">A collection of identifiers for the records to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task yielding a collection of records matching the provided identifiers.</returns>
    Task<IEnumerable<TModel>> FindManyByIdsAsync(IEnumerable<long> ids, CancellationToken cancellationToken = default);
}