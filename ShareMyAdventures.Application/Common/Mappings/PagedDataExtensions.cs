// Application Project: Mappings/PagedDataExtensions.cs
using Microsoft.EntityFrameworkCore;
using ShareMyAdventures.Application.Common.Models;
using System.Linq;

namespace ShareMyAdventures.Application.Common.Mappings;

/// <summary>
/// Provides extension methods for creating paginated data from collections.
/// </summary>
public static class PagedDataExtensions
{
    /// <summary>
    /// Asynchronously paginates an <see cref="IQueryable{T}"/> collection into a <see cref="PagedData{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the destination data, must be a reference type.</typeparam>
    /// <param name="source">The queryable collection to paginate.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="map">An optional function to map each item to a new type (e.g., DTO).</param>
    /// <returns>A task representing the asynchronous operation, containing a <see cref="PagedData{T}"/> with the paginated data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> or <paramref name="pageSize"/> is less than 1.</exception>
    /// <remarks>
    /// If the collection is empty or the page number exceeds available pages, an empty <see cref="PagedData{T}"/> is returned.
    /// </remarks>
    public static async Task<PagedData<T>> ToPagedDataAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize)
        where T : class
    {
        if (source == null) 
            throw new ArgumentNullException(nameof(source), "Source collection cannot be null");
        if (pageNumber < 1) 
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be 1 or greater");
        if (pageSize < 1) 
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be 1 or greater");

        var pagedData = await PagedData<T>.CreateAsync(source.AsNoTracking(), pageNumber, pageSize);

            return new PagedData<T>(
                pagedData.PageNo,
                pagedData.PageSize,
                pagedData.ItemCount,
                source);
    }

    public static PagedData<T> ToPagedData<T>(
       this IEnumerable<T> source,
       int pageNumber,
       int pageSize)
       where T : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source), "Source collection cannot be null");
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be 1 or greater");
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be 1 or greater");

        var pagedData = PagedData<T>.Create(source, pageNumber, pageSize);

        return new PagedData<T>(
            pagedData.PageNo,
            pagedData.PageSize,
            pagedData.ItemCount,
            source);
    }
}