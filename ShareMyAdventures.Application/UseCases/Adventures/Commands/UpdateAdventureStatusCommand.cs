using ShareMyAdventures.Application.Common.Exceptions;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Application.UseCases.Adventures.Commands;

public sealed record UpdateAdventureStatusCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
    public int StatusLookupId { get; init; } = 0;
}

internal sealed class UpdateAdventureStatusCommandValidator : AbstractValidator<UpdateAdventureStatusCommand>
{
    private readonly IAdventureRepository _adventureRepository;
    private readonly ICurrentUser _currentUserService;

    internal UpdateAdventureStatusCommandValidator(IAdventureRepository adventureRepository, ICurrentUser currentUserService)
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

        return await _adventureRepository.HasActiveAdventuresAsync(id, _currentUserService.UserId, cancellationToken); 
    }
}


public sealed class UpdateAdventureStatusCommandHandler(
    IAdventureRepository adventureRepository,
    ICurrentUser currentUserService)
    : IRequestHandler<UpdateAdventureStatusCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAdventureStatusCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateAdventureStatusCommandValidator(adventureRepository, currentUserService);
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var entity = await adventureRepository.GetByIdAsync(request.Id, cancellationToken);
        entity = entity.ThrowIfNotFound(request.Id);

        if (entity.CreatedBy != userId)
        {
            throw new ForbiddenAccessException("User is not allowed to update the adventure.");
        }

        var status = StatusLookup.All.FirstOrDefault(x => x.Id == request.StatusLookupId);
        status = status.ThrowIfNotFound(request.StatusLookupId);

        entity.UpdateStatus(status);

        await adventureRepository.UpdateAsync(entity, cancellationToken); 

        return Unit.Value;
    }
}
