using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.Adventures.Commands;

public sealed record DeleteAdventureCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
}

public sealed class DeleteAdventureCommandHandler(
    IReadRepository<Adventure> adventureReadableRepository,
    IWriteRepository<Adventure> adventureRepository,
    ICurrentUser currentUserService
    ) : IRequestHandler<DeleteAdventureCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAdventureCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var entity = await adventureReadableRepository.FindOneByIdAsync(request.Id, cancellationToken);

        if (entity?.CreatedBy != userId)
        {
            throw new UnauthorizedAccessException("User is not the administrator");
        }

        await adventureRepository.DeleteAsync(entity, cancellationToken);
        await adventureRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
