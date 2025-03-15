using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Application.Common.Interfaces.Repositories;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Application.UseCases.Adventures.Commands;

public sealed record CreateAdventureCommand : IRequest<Result<long?>>
{
    public string Name { get; init; } = string.Empty;
    public int TypeLookupId { get; init; } = 0;
    public DateTime StartDate { get; init; } = DateTime.UtcNow;
    public DateTime EndDate { get; init; } = DateTime.UtcNow;
    public long? MeetupLocationId { get; init; }
    public long? DestinationLocationId { get; init; }
}

internal sealed class CreateAdventureCommandValidator : AbstractValidator<CreateAdventureCommand>
{
    internal CreateAdventureCommandValidator()
    {
        RuleFor(v => v.StartDate).NotEmpty();
        RuleFor(v => v.EndDate).NotEmpty();
        RuleFor(v => v.TypeLookupId)
            .GreaterThan(0)
            .Must(CheckTypeIdExist)
            .WithMessage("The Type Id provided does not exist.");

        RuleFor(v => v.Name).MinimumLength(5).MaximumLength(32);

        RuleFor(x => x.StartDate).GreaterThan(DateTime.Now);
        RuleFor(x => x.EndDate).GreaterThan(DateTime.Now);
    }

    private static bool CheckTypeIdExist(int id)
    {
        var entities = TypeLookup.All;

        return entities.Any(x => x.Id == id);
    }
}


public sealed class CreateAdventureCommandHandler(
    IAdventureRepository adventureRepository,
    ICurrentUser currentUserService,
    IParticipantRepository participantRepository
        ) : IRequestHandler<CreateAdventureCommand, Result<long?>>
{

    public async Task<Result<long?>> Handle(CreateAdventureCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateAdventureCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var userId = currentUserService.UserId.ThrowIfNullOrEmpty("User Id");

        var participant = await userManager.FindByIdAsync(userId);
        participant = participant.ThrowIfNotFound(userId);

        var adventure = new Adventure
        {
            DestinationLocationId = request.DestinationLocationId,
            MeetupLocationId = request.MeetupLocationId,
            // make the dates UCT date time
            StartDate = request.StartDate.ToUniversalTime(),
            EndDate = request.EndDate.ToUniversalTime(),
            StatusLookupId = StatusLookups.Created.Id,
           TypeLookupId = request.TypeLookupId
        };

        adventure.AddParticipant(new ParticipantAdventure
        {
            Participant = participant,
            AccessLevelLookupId = Domain.Enums.AccessLevelLookups.Administrator.Id
        });

        await adventureRepository.AddAsync(adventure, cancellationToken);
        await adventureRepository.SaveChangesAsync(cancellationToken);

        return Result<long?>.Success(adventure.Id);
    }
}

