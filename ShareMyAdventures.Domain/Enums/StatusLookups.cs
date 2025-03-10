using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Enums;

public sealed class StatusLookups : BaseEnum
{
    public static readonly StatusLookups Created = new(nameof(Created), 1);
    public static readonly StatusLookups InProgress = new("In Progress", 2);
    public static readonly StatusLookups Completed = new(nameof(Completed), 3);

    private StatusLookups(string name, int value) : base(value, name)
    {
    }
    public static StatusLookups[] List
    {
        get { return [Created, InProgress, Completed]; }
    }
}

