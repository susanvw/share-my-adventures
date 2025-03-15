using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;

namespace ShareMyAdventures.Application.UseCases.Friends.Commands;

public sealed record DeleteFriendCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
}

public sealed class DeleteParticipantFriendCommandHandler(
    IParticipantRepository participantRepository,
    ICurrentUser currentUser
    ) : IRequestHandler<DeleteFriendCommand, Unit>
{

    public async Task<Unit> Handle(DeleteFriendCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        userId = userId.ThrowIfNullOrWhiteSpace("Current User");

        var entity = await participantRepository
            .GetByIdAsync(userId, cancellationToken);
        entity = entity.ThrowIfNotFound(userId);

        var friendRequest = entity.FindFriendRequest(request.Id);
        friendRequest = friendRequest.ThrowIfNotFound(request.Id);

        entity.RemoveFriendRequest(friendRequest);

        await participantRepository.UpdateAsync(entity, cancellationToken);

        return Unit.Value;
    }
}
