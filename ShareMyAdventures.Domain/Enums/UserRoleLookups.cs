using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Enums;

public sealed class UserRoleLookups : BaseEnum
{
    public static readonly UserRoleLookups Administrator = new(nameof(Administrator), 1);
    public static readonly UserRoleLookups User = new(nameof(User), 2);

    private UserRoleLookups(string name, int value) : base(value, name)
    {
    }

    public static UserRoleLookups[] List
    {
        get { return [Administrator, User]; }
    }
}

