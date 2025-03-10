using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

public sealed class StatusLookup : ValueObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
