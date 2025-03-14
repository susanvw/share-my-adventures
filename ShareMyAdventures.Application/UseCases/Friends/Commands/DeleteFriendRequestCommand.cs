using FluentValidation.Results;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.Friends.Commands;

public sealed record DeleteFriendRequestCommand : IRequest<Unit>
{
    public long Id { get; init; } 
}

public sealed class DeleteFriendRequestCommandHandler(
    IParticipantRepository manager,
    ICurrentUser currentUser
    ) : IRequestHandler<DeleteFriendRequestCommand, Unit>
{
    public async Task<Unit> Handle(DeleteFriendRequestCommand request, CancellationToken cancellationToken)
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
            if (friendRequest.InvitationStatusLookupId != Domain.Enums.InvitationStatusLookups.Pending.Id)
            {
                var validationFailure = new ValidationFailure("Invitation Status", "Cannot delete an invitation that is not in pending state.");
                throw new Common.Exceptions.ValidationException([validationFailure]);
            }

            entity.RemoveFriendRequest(friendRequest);

            await repository.UpdateAsync(entity, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }
        return Unit.Value;
    }
}
