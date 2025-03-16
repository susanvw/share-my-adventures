using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.Adventures.Commands;

public sealed record DeleteAdventureCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
}

public sealed class DeleteAdventureCommandHandler(
    IAdventureRepository adventureRepository,
    ICurrentUser currentUserService
    ) : IRequestHandler<DeleteAdventureCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAdventureCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var entity = await adventureRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity?.CreatedBy != userId)
        {
            throw new UnauthorizedAccessException("User is not the administrator");
        }

        await adventureRepository.RemoveAsync(entity, cancellationToken);

        return Unit.Value;
    }
}
