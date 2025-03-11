using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

/// <summary>
/// Represents a friend list created by a participant in the ShareMyAdventures domain.
/// Managed through the <see cref="Participant"/> aggregate root.
/// </summary>
public sealed class FriendList : BaseAuditableEntity
{
    private readonly List<Participant> _friends = [];

    /// <summary>
    /// Gets the name of the friend list.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets a read-only collection of participants in this friend list.
    /// </summary>
    public IReadOnlyCollection<Participant> Friends => _friends.AsReadOnly();

    // EF Core parameterless constructor
    private FriendList() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FriendList"/> class.
    /// </summary>
    /// <param name="name">The name of the friend list.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
    public FriendList(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    /// Adds a participant to the friend list if not already present.
    /// </summary>
    /// <param name="friend">The participant to add.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="friend"/> is null.</exception>
    public void AddFriend(Participant friend)
    {
        ArgumentNullException.ThrowIfNull(friend, nameof(friend));
        if (!_friends.Any(f => f.Id == friend.Id))
        {
            _friends.Add(friend);
        }
    }

    /// <summary>
    /// Removes a participant from the friend list.
    /// </summary>
    /// <param name="friend">The participant to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="friend"/> is null.</exception>
    public void RemoveFriend(Participant friend)
    {
        ArgumentNullException.ThrowIfNull(friend, nameof(friend));
        _friends.Remove(friend);
    }
}