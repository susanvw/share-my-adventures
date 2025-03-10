using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

public sealed class TypeLookup : ValueObject
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
