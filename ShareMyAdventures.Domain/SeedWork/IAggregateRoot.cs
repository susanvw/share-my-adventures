namespace ShareMyAdventures.Domain.SeedWork;

/// <summary>
/// Marker interface for identifying aggregate roots in the domain model.
/// Aggregate roots are entities that define the boundaries of a consistency model
/// and serve as the entry point for accessing and modifying related domain objects.
/// </summary>
public interface IAggregateRoot { }