using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Application.UseCases.Friends.Commands;

public sealed record DeleteFriendCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
}

public sealed class DeleteParticipantFriendCommandHandler(
    IReadableRepository<FriendRequest> readableRepository,
    IWriteRepository<FriendRequest> repository) : IRequestHandler<DeleteFriendCommand, Unit>
{

    public async Task<Unit> Handle(DeleteFriendCommand request, CancellationToken cancellationToken)
    {
        var entity = await readableRepository.FindOneByIdAsync(request.Id, cancellationToken);
        entity = entity.ThrowIfNotFound(request.Id);

        await repository.DeleteAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
