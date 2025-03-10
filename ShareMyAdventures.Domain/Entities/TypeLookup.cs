using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Domain.Entities;

public sealed class TypeLookup : IAggregateRoot
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
