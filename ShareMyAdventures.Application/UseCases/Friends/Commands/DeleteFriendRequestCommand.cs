using FluentValidation.Results;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Application.UseCases.Friends.Commands;

public sealed record DeleteFriendRequestCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
}

public sealed class DeleteFriendRequestCommandHandler(
    IReadableRepository<FriendRequest> friendRequestRepository,
    IWriteRepository<FriendRequest> friendRepository
    ) : IRequestHandler<DeleteFriendRequestCommand, Unit>
{
    public async Task<Unit> Handle(DeleteFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await friendRequestRepository.FindOneByIdAsync(request.Id, cancellationToken);
        entity = entity.ThrowIfNotFound(request.Id);

        if (entity.InvitationStatusLookupId != Domain.Enums.InvitationStatusLookups.Pending.Id)
        {
            var validationFailure = new ValidationFailure("Invitation Status", "Cannot delete an invitation that is not in pending state.");
            throw new Common.Exceptions.ValidationException([validationFailure]);
        }

        await friendRepository.DeleteAsync(entity, cancellationToken);
        await friendRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
