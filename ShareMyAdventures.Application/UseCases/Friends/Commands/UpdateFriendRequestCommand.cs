using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.Enums;
using ShareMyAdventures.Domain.SeedWork;

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
        var statuses = BaseEnum.GetAll<AccessLevelLookups>();

        if (statuses.Any(x => x.Id == id)) return true;

        return false;
    }
}

public class UpdateFriendRequestCommandHandler(
    IReadRepository<FriendRequest> readableRepository,
    IWriteRepository<FriendRequest> repository) : IRequestHandler<UpdateFriendRequestCommand, Unit>
{
    public async Task<Unit> Handle(UpdateFriendRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await readableRepository.FindOneByIdAsync(request.Id, cancellationToken);
        entity = entity.ThrowIfNotFound(request.Id);

        entity.InvitationStatusLookupId = request.InvitationStatusLookupId;

        await repository.UpdateAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken)
;
        return Unit.Value;
    }
}