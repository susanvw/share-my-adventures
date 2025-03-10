// Application Project: Models/PagedData.cs
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ShareMyAdventures.Application.Common.Models;

/// <summary>
/// Represents a paged set of data.
/// </summary>
public sealed class PagedData<T> where T: class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedData{T}"/> class.
    /// </summary>
    /// <param name="pageNo">The current page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="count">The total number of items in the dataset.</param>
    /// <param name="data">The current page's data, or null if empty.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNo"/> or <paramref name="pageSize"/> is less than 1.</exception>
    public PagedData(int pageNo, int pageSize, int count, IEnumerable<T>? data)
    {
        if (pageNo < 1) 
            throw new ArgumentOutOfRangeException(nameof(pageNo), "Page number must be 1 or greater");
        if (pageSize < 1) 
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be 1 or greater");
        if (count < 0) 
            throw new ArgumentOutOfRangeException(nameof(count), "Item count cannot be negative");

        PageNo = pageNo;
        PageSize = pageSize;
        ItemCount = count;
        Data = data ?? [];
        PageCount = (int)Math.Ceiling(count / (double)pageSize);
    }

    /// <summary>
    /// Gets or sets the current page number of the dataset (1-based).
    /// </summary>
    public int PageNo { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page of the dataset.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages in the dataset.
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of items in the dataset.
    /// </summary>
    public int ItemCount { get; set; }

    /// <summary>
    /// Gets or sets the data of the current page.
    /// </summary>
    public IEnumerable<T> Data { get; set; }

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage => PageNo > 1;

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    public bool HasNextPage => PageNo < PageCount;

    /// <summary>
    /// Creates an instance of <see cref="PagedData{T}"/> synchronously from an <see cref="IEnumerable{T}"/> source.
    /// </summary>
    /// <param name="source">The enumerable data source to paginate.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>The paged data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> or <paramref name="pageSize"/> is less than 1.</exception>
    public static PagedData<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        if (source == null) 
            throw new ArgumentNullException(nameof(source), "Source cannot be null");
        if (pageNumber < 1) 
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be 1 or greater");
        if (pageSize < 1) 
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be 1 or greater");

        var items = source.ToList(); // Single enumeration
        var itemCount = items.Count;
        var chunks = items.Chunk(pageSize);
        var pageItems = chunks.ElementAtOrDefault(pageNumber - 1);

        return new PagedData<T>(pageNumber, pageSize, itemCount, pageItems);
    }

    /// <summary>
    /// Creates an instance of <see cref="PagedData{T}"/> asynchronously from an <see cref="IQueryable{T}"/> source.
    /// </summary>
    /// <param name="source">The queryable data source to paginate.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A task representing the asynchronous operation, containing the paged data.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageNumber"/> or <paramref name="pageSize"/> is less than 1.</exception>
    public static async Task<PagedData<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (source == null) 
            throw new ArgumentNullException(nameof(source), "Source cannot be null");
        if (pageNumber < 1) 
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be 1 or greater");
        if (pageSize < 1) 
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be 1 or greater");

        var itemCount = await source.CountAsync(cancellationToken: cancellationToken);
        var takenItems = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedData<T>(pageNumber, pageSize, itemCount, takenItems);
    }
}