using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Enums;

public sealed class AccessLevelLookups : BaseEnum
{
    public static readonly AccessLevelLookups Viewer = new(nameof(Viewer), 1);
    public static readonly AccessLevelLookups Participant = new(nameof(Participant), 2);
    public static readonly AccessLevelLookups Administrator = new(nameof(Administrator), 3);

    private AccessLevelLookups(string name, int value) : base(value, name)
    {
    }


    public static AccessLevelLookups[] List
    {
        get { return [Viewer, Participant, Administrator]; }
    }
}
