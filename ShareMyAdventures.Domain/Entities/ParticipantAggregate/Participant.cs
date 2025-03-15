using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

/// <summary>
/// Represents a participant in the ShareMyAdventures domain, serving as an aggregate root.
/// Inherits from IdentityUser for authentication and manages friends, friend lists, positions, and notifications.
/// </summary>
public sealed class Participant : IdentityUser, IAggregateRoot
{
    private readonly List<ParticipantAdventure> _adventures = [];
    private readonly List<FriendRequest> _friends = [];
    private readonly List<FriendList> _friendLists = [];
    private readonly List<Position> _positions = [];
    private readonly List<Notification> _notifications = [];

    /// <summary>
    /// Gets or sets the refresh token for authentication.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the expiry time of the refresh token.
    /// </summary>
    public DateTime RefreshTokenExpiryTime { get; set; }

    /// <summary>
    /// Gets the display name of the participant.
    /// </summary>
    public string DisplayName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the URL or path to the participant’s photo, if provided.
    /// </summary>
    public string? Photo { get; private set; }

    /// <summary>
    /// Gets a value indicating whether other participants can follow this participant’s position.
    /// </summary>
    public bool FollowMe { get; private set; }

    /// <summary>
    /// Gets the trail color used for this participant on maps, if specified.
    /// </summary>
    public string? TrailColor { get; private set; }

    /// <summary>
    /// Gets a read-only collection of adventures this participant is involved in.
    /// </summary>
    public IReadOnlyCollection<ParticipantAdventure> Adventures => _adventures.AsReadOnly();

    /// <summary>
    /// Gets a read-only collection of friend requests sent or received by this participant.
    /// </summary>
    public IReadOnlyCollection<FriendRequest> Friends => _friends.AsReadOnly();

    /// <summary>
    /// Gets a read-only collection of friend lists created by this participant.
    /// </summary>
    public IReadOnlyCollection<FriendList> FriendLists => _friendLists.AsReadOnly();

    /// <summary>
    /// Gets a read-only collection of position updates for this participant.
    /// </summary>
    public IReadOnlyCollection<Position> Positions => _positions.AsReadOnly();

    /// <summary>
    /// Gets a read-only collection of notifications for this participant.
    /// </summary>
    public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

    // EF Core parameterless constructor (required by Identity and EF)
    public Participant() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Participant"/> class.
    /// </summary>
    /// <param name="userName">The username (inherited from IdentityUser).</param>
    /// <param name="displayName">The display name of the participant.</param>
    public Participant(string userName, string displayName)
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
    }

    /// <summary>
    /// Updates the participant’s profile details.
    /// </summary>
    /// <param name="displayName">The new display name.</param>
    /// <param name="photo">The new photo URL or path, if provided.</param>
    /// <param name="followMe">Whether others can follow this participant’s position.</param>
    /// <param name="trailColor">The new trail color, if specified.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="displayName"/> is null.</exception>
    public void UpdateProfile(string displayName, string? photo = null, bool followMe = false, string? trailColor = null)
    {
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
        Photo = photo;
        FollowMe = followMe;
        TrailColor = trailColor;
    }

    /// <summary>
    /// Finds a friend request by its ID.
    /// </summary>
    /// <param name="id">The ID of the friend request to find.</param>
    /// <returns>The matching friend request if found; otherwise, null.</returns>
    public FriendRequest? FindFriendRequest(long id)
    {
        return _friends.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Removes a friend request from the participant’s list.
    /// </summary>
    /// <param name="friendRequest">The friend request to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="friendRequest"/> is null.</exception>
    public void RemoveFriendRequest(FriendRequest friendRequest)
    {
        ArgumentNullException.ThrowIfNull(friendRequest, nameof(friendRequest));
        _friends.Remove(friendRequest);
    }

    /// <summary>
    /// Adds a friend request to the participant’s list if not already present.
    /// </summary>
    /// <param name="friendRequest">The friend request to add.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="friendRequest"/> is null.</exception>
    public void AddFriendRequest(FriendRequest friendRequest)
    {
        ArgumentNullException.ThrowIfNull(friendRequest);
        if (!_friends.Any(f => f.Id == friendRequest.Id))
        {
            _friends.Add(friendRequest);
        }
    }

    public void AddPosition(Position entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _positions.Add(entity);
    }
}