using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;

namespace ShareMyAdventures.Application.UseCases.Friends.Commands;

public sealed record DeleteFriendRequestCommand : IRequest<Unit>
{
    public long Id { get; init; } 
}

public sealed class DeleteFriendRequestCommandHandler(
    IParticipantRepository participantRepository,
    ICurrentUser currentUser
    ) : IRequestHandler<DeleteFriendRequestCommand, Unit>
{
    public async Task<Unit> Handle(DeleteFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        userId = userId.ThrowIfNullOrWhiteSpace("Current User");

        var entity = await participantRepository.GetByIdAsync(userId, cancellationToken);
        entity = entity.ThrowIfNotFound(userId);

        var friendRequest = entity.FindFriendRequest(request.Id);

        if (friendRequest != null)
        {
            entity.RemoveFriendRequest(friendRequest);

            await participantRepository.UpdateAsync(entity, cancellationToken);
        }
        return Unit.Value;
    }
}
