using ShareMyAdventures.Application.Common.Exceptions;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Application.UseCases.Adventures.Commands;

public sealed record UpdateAdventureStatusCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
    public int StatusLookupId { get; init; } = 0;
}

internal sealed class UpdateAdventureStatusCommandValidator : AbstractValidator<UpdateAdventureStatusCommand>
{
    private readonly IReadableRepository<Adventure> _adventureRepository;
    private readonly ICurrentUser _currentUserService;

    internal UpdateAdventureStatusCommandValidator(IReadableRepository<Adventure> adventureRepository, ICurrentUser currentUserService)
    {

        _adventureRepository = adventureRepository;
        _currentUserService = currentUserService;

        RuleFor(v => v.Id).GreaterThan(0);
        RuleFor(v => v.StatusLookupId)
        .GreaterThan(0)
            .MustAsync(CheckOnlyOneActiveAdventure)
            .WithMessage("Only one adventure is allowed to be in progress.");
    }

    private async Task<bool> CheckOnlyOneActiveAdventure(int id, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId == null)
            return false;

        var adventure = await _adventureRepository
            .Include(x => x.Participants)
            .FindByCustomFilterAsync(x =>
                x.Id == id &&
                x.StatusLookupId == Domain.Enums.StatusLookups.InProgress.Id &&
                x.Participants.Any(x => x.ParticipantId == _currentUserService.UserId
             ),
            cancellationToken);

        return adventure.Any();
    }
}


public sealed class UpdateAdventureStatusCommandHandler(
    IReadableRepository<Adventure> adventureReadableRepository,
    IWriteRepository<Adventure> adventureRepository,
    ICurrentUser currentUserService)
    : IRequestHandler<UpdateAdventureStatusCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAdventureStatusCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateAdventureStatusCommandValidator(adventureReadableRepository, currentUserService);
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var entity = await adventureReadableRepository.FindOneByIdAsync(request.Id, cancellationToken);
        entity = entity.ThrowIfNotFound(request.Id);

        if (entity.CreatedBy != userId)
        {
            throw new ForbiddenAccessException("User is not allowed to update the adventure.");
        }

        entity.StatusLookupId = request.StatusLookupId;

        await adventureRepository.UpdateAsync(entity, cancellationToken);
        await adventureRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
