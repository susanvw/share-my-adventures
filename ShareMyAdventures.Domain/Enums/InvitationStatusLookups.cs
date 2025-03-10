using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Enums;

public sealed class InvitationStatusLookups : BaseEnum
{
    public static readonly InvitationStatusLookups Pending = new(nameof(Pending), 1);
    public static readonly InvitationStatusLookups Accepted = new(nameof(Accepted), 2);
    public static readonly InvitationStatusLookups Rejected = new(nameof(Rejected), 3);

    private InvitationStatusLookups(string name, int value) : base(value, name)
    {
    }

    public static InvitationStatusLookups[] List
    {
        get { return [Pending, Accepted, Rejected]; }
    }
}
