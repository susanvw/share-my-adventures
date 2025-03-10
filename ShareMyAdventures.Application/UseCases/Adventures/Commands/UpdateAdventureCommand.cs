using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Application.UseCases.Adventures.Commands;

public sealed record UpdateAdventureCommand : IRequest<Unit>
{
    public long Id { get; init; } = 0;
    public string Name { get; init; } = string.Empty;
    public int TypeLookupId { get; init; } = 0;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public long? MeetupLocationId { get; init; }
    public long? DestinationLocationId { get; init; }
}

internal sealed class UpdateAdventureCommandValidator : AbstractValidator<UpdateAdventureCommand>
{

    internal UpdateAdventureCommandValidator()
    {
        RuleFor(v => v.StartDate).GreaterThan(DateTime.Now);
        RuleFor(v => v.EndDate).GreaterThan(DateTime.Now);

        RuleFor(v => v.TypeLookupId)
            .GreaterThan(0)
            .Must(CheckTypeIdExist)
            .WithMessage("The Adventure Type Id does not exist.");

        RuleFor(v => v.Id).GreaterThan(0);
        RuleFor(v => v.Name).MinimumLength(5).MaximumLength(32);
    }

    private static bool CheckTypeIdExist(int id)
    {
        var types = Domain.Enums.TypeLookups.GetAll<Domain.Enums.TypeLookups>();

        if (types.Any(x => x.Id == id))
        {
            return true;
        }

        return false;
    }
}

public sealed class UpdateAdventureCommandHandler(
    IReadableRepository<Adventure> adventureReadableRepository,
    IWriteRepository<Adventure> adventureRepository,
    ICurrentUser currentUserService) 
    : IRequestHandler<UpdateAdventureCommand, Unit>
{

    public async Task<Unit> Handle(UpdateAdventureCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateAdventureCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("Current User");

        var entity = await adventureReadableRepository.FindOneByIdAsync(request.Id, cancellationToken);

        if (entity?.CreatedBy != userId)
        {
            throw new UnauthorizedAccessException("User does not have access to the adventure.");
        }

        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.Name = request.Name;
        entity.DestinationLocationId = request.DestinationLocationId;
        entity.MeetupLocationId = request.MeetupLocationId;
        entity.TypeLookupId = request.TypeLookupId;

        await adventureRepository.UpdateAsync(entity, cancellationToken);
        await adventureRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}