using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.Friends.Commands;

public sealed record DeleteFriendCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
}

public sealed class DeleteParticipantFriendCommandHandler(
    IReadRepository<Participant> readRepository,
    IWriteRepository<Participant> repository,
    ICurrentUser currentUser
    ) : IRequestHandler<DeleteFriendCommand, Unit>
{

    public async Task<Unit> Handle(DeleteFriendCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        userId = userId.ThrowIfNullOrWhiteSpace("Current User");

        var entity = await readRepository
                .Include(x => x.Friends)
                .FindOneByCustomFilterAsync(x => x.Id == userId, cancellationToken);
        entity = entity.ThrowIfNotFound(userId);

        var friendRequest = entity.FindFriendRequest(request.Id);

        if (friendRequest != null)
        {
            entity.RemoveFriendRequest(friendRequest);

            await repository.UpdateAsync(entity, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}
