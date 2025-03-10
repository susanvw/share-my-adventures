// Application Project: Models/SortDirection.cs
namespace ShareMyAdventures.Application.Common.Models;

/// <summary>
/// Defines the direction in which data should be sorted.
/// </summary>
public enum SortDirection
{
    /// <summary>
    /// Sort in ascending order (e.g., A to Z, 1 to 9).
    /// </summary>
    Ascending,

    /// <summary>
    /// Sort in descending order (e.g., Z to A, 9 to 1).
    /// </summary>
    Descending
}