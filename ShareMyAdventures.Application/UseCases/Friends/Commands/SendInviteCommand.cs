using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Friends.Commands;

public sealed record SendInviteCommand : IRequest<Result<long?>>
{
    public string UserId { get; init; } = string.Empty;
}

internal class SendInviteCommandValidator : AbstractValidator<SendInviteCommand>
{
    internal SendInviteCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty();
    }
}


public sealed class SendInviteCommandHandler(
    ICurrentUser currentUserService,
    IParticipantRepository participantRepository
    ) : IRequestHandler<SendInviteCommand, Result<long?>>
{
    public async Task<Result<long?>> Handle(SendInviteCommand request, CancellationToken cancellationToken)
    {
        var validator = new SendInviteCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var userId = currentUserService.UserId.ThrowIfNullOrWhiteSpace("Current User");

        // get the friend
        var friend = await participantRepository.GetByIdAsync(request.UserId);
        friend = friend.ThrowIfNotFound(request.UserId);

        // get current user
        var participant = await participantRepository.GetByIdAsync(userId);
        participant = participant.ThrowIfNotFound(userId);


        // check if the relationship already exists
        var existing = await participantRepository.HasBeenInvitedAsync(participant, friend, cancellationToken);

        if (existing)
        {
            return Result<long?>.Failure(["Cannot invite the same person"]);
        }

        var entity = new FriendRequest(participant.Id, friend.Id, InvitationStatusLookup.Pending);

        participant.AddFriendRequest(entity);

        await participantRepository.UpdateAsync(entity, cancellationToken);

        return Result<long?>.Success(entity.Id);

    }
}
