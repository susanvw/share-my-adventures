// Application Project: Mappings/PagedDataExtensions.cs
using Microsoft.EntityFrameworkCore; // Provides EF Core extension methods like CountAsync, ToListAsync
using ShareMyAdventures.Application.Common.Models; // Contains the PagedData<T> model

namespace ShareMyAdventures.Application.Common.Mappings; // Namespace for mapping-related extensions

/// <summary>
/// Provides extension methods for creating paginated data from IQueryable sources.
/// </summary>
public static class PagedDataExtensions
{
    /// <summary>
    /// Asynchronously converts an IQueryable source into a PagedData object with pagination applied.
    /// </summary>
    /// <typeparam name="T">The type of the data being paginated, constrained to reference types.</typeparam>
    /// <param name="source">The IQueryable source to paginate, typically an entity query from a repository.</param>
    /// <param name="pageNumber">The 1-based page number to retrieve (e.g., 1 for the first page).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    /// <returns>A Task containing a PagedData<T> instance with the paginated results.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the source parameter is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if pageNumber or pageSize is less than 1.</exception>
    public static async Task<PagedData<T>> ToPagedDataAsync<T>(
        this IQueryable<T> source, // Extension method applied to IQueryable<T>
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default) where T : class
    {
        // Validate that the source query is not null to prevent null reference exceptions
        if (source == null) throw new ArgumentNullException(nameof(source), "The source query cannot be null.");

        // Ensure pageNumber is valid (1 or greater) since negative or zero pages are illogical
        if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be 1 or greater.");

        // Ensure pageSize is valid (1 or greater) to avoid invalid pagination requests
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be 1 or greater.");

        // Get the total count of items in the source query before applying pagination
        // This executes a COUNT(*) SQL query to determine the total number of records
        var count = await source.CountAsync(cancellationToken);

        // Apply pagination to the source query:
        // - Skip: Calculates the number of items to skip based on the page number (0-based offset)
        // - Take: Limits the result set to the specified page size, translated to SQL (e.g., TOP or FETCH)
        // - ToListAsync: Executes the query and materializes the results into a List<T>
        var data = await source
            .Skip((pageNumber - 1) * pageSize) // E.g., page 2 with size 10 skips 10 items
            .Take(pageSize) // Takes the next 'pageSize' items efficiently at the database level
            .ToListAsync(cancellationToken); // Converts the IQueryable to a materialized List<T>

        // Construct and return a PagedData<T> instance with the paginated results
        // Includes page number, page size, total item count, and the current page's data
        return new PagedData<T>(pageNumber, pageSize, count, data);
    }
}