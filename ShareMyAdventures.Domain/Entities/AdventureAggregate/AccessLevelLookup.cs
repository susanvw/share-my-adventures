using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

/// <summary>
/// Access Level lookup 
/// Indicate what access level a particpants has for an adventure
/// </summary>
public sealed class AccessLevelLookup: ValueObject
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
