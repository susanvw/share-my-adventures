using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Enums;

public sealed class TypeLookups : Enumeration
{
    public static readonly TypeLookups Run = new(nameof(Run), 1);
    public static readonly TypeLookups Walk = new(nameof(Walk), 2);
    public static readonly TypeLookups Cycle = new(nameof(Cycle), 3);
    public static readonly TypeLookups RoadTrip = new("Road Trip", 4);
    public static readonly TypeLookups Gathering = new(nameof(Gathering), 5);

    private TypeLookups(string name, int value) : base(value, name)
    {
    }

    public static TypeLookups[] List
    {
        get { return [Run, Walk, Cycle, RoadTrip, Gathering]; }
    }
}
