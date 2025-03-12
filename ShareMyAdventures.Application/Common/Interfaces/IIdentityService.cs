using ShareMyAdventures.Application.UseCases.Friends.Queries;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.Common.Interfaces;

/// <summary>
/// Defines a service for identity and participant-related operations.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Gets the username for a given user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The username, or null if not found.</returns>
    Task<string?> GetUserNameAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user is in a specific role.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="role">The role to check.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>True if the user is in the role, false otherwise.</returns>
    Task<bool> IsInRoleAsync(string userId, string role, CancellationToken cancellationToken = default);

    /// <summary>
    /// Authorizes a user against a policy.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="policyName">The name of the policy.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>True if authorized, false otherwise.</returns>
    Task<bool> AuthorizeAsync(string userId, string policyName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a participant by their ID, including basic Identity data.
    /// </summary>
    /// <param name="userId">The ID of the participant.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The participant, or null if not found.</returns>
    Task<Participant?> GetParticipantAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paged list of pending friend requests for a participant.
    /// </summary>
    /// <param name="participantId">The ID of the participant.</param>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A paged list of friend requests.</returns>
    Task<PagedData<FriendRequestView>> GetPendingFriendRequestsAsync(string participantId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}