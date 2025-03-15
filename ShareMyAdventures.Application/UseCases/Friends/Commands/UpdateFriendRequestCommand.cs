using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Application.UseCases.Friends.Commands;

public sealed record UpdateFriendRequestCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
    public int InvitationStatusLookupId { get; init; } = 0;
}

internal class UpdateFriendRequestCommandValidator : AbstractValidator<UpdateFriendRequestCommand>
{
    internal UpdateFriendRequestCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.InvitationStatusLookupId)
            .GreaterThan(0)
            .Must(CheckInvitationStatusIdExist)
            .WithMessage("The Invitation Status Id does not exist.");
    }

    private static bool CheckInvitationStatusIdExist(int id)
    {
        var statuses = AccessLevelLookup.All;

        if (statuses.Any(x => x.Id == id)) return true;

        return false;
    }
}

public class UpdateFriendRequestCommandHandler(IParticipantRepository participantRepository, ICurrentUser currentUser) : IRequestHandler<UpdateFriendRequestCommand, Unit>
{
    public async Task<Unit> Handle(UpdateFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUser.UserId.ThrowIfNullOrWhiteSpace("Current User");

        var entity = await participantRepository.GetByIdAsync(currentUserId, cancellationToken);
        entity = entity.ThrowIfNotFound(request.Id);

        var friendRequest = entity.FindFriendRequest(request.Id);
        friendRequest = friendRequest.ThrowIfNotFound(request.Id);

        var status = InvitationStatusLookup.All.FirstOrDefault(x => x.Id == request.InvitationStatusLookupId);
        status = status.ThrowIfNotFound(request.InvitationStatusLookupId);

        friendRequest.UpdateStatus(status);

        await participantRepository.UpdateAsync(entity, cancellationToken);

        return Unit.Value;
    }
}