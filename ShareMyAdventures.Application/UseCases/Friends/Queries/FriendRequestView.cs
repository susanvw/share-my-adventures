using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Friends.Queries;

/// <summary>
/// Represents a view model for a friend request, tailored for display purposes.
/// </summary>
public sealed record FriendRequestView
{
    /// <summary>
    /// Gets the ID of the friend request.
    /// </summary>
    public long Id { get; internal set; }

    /// <summary>
    /// Gets the display name of the participant involved in the friend request.
    /// </summary>
    public string DisplayName { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the URL or path to the participant’s photo, if available.
    /// </summary>
    public string? Photo { get; internal set; }

    /// <summary>
    /// Gets the ID of the invitation status lookup.
    /// </summary>
    public int InvitationStatusLookupId { get; internal set; }

    /// <summary>
    /// Gets the name of the invitation status lookup.
    /// </summary>
    public string InvitationStatusLookupName { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the current user can accept this friend request.
    /// </summary>
    public bool CanAccept { get; internal set; }

    /// <summary>
    /// Maps a <see cref="FriendRequest"/> entity to a <see cref="FriendRequestView"/>.
    /// </summary>
    /// <param name="entity">The friend request entity to map.</param>
    /// <param name="currentParticipantId">The ID of the current participant.</param>
    /// <returns>A new instance of <see cref="FriendRequestView"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is null.</exception>
    internal static FriendRequestView MapFrom(FriendRequest entity, string currentParticipantId)
    {
        ArgumentNullException.ThrowIfNull(entity);

        // Determine if the current user is the requester or the recipient
        var isRequester = entity.ParticipantId == currentParticipantId;
        var targetParticipant = isRequester ? entity.ParticipantFriend : entity.Participant;

        return new FriendRequestView
        {
            Id = entity.Id,
            DisplayName = targetParticipant.DisplayName,
            Photo = targetParticipant.Photo, // Assuming Photo is a string URL, not byte[]
            InvitationStatusLookupId = entity.InvitationStatusLookup.Id,
            InvitationStatusLookupName = entity.InvitationStatusLookup.Name,
            CanAccept = !isRequester && entity.InvitationStatusLookup.Id == InvitationStatusLookup.Pending.Id
        };
    }
}