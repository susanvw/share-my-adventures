using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Application.Common.Guards;
using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Positions.Commands;

public record CreatePositionCommand : IRequest<Result<long?>>
{
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public float? Speed { get; set; }
    public float? Heading { get; set; }
    public float? Altitude { get; set; }
    public string? TimeStamp { get; set; }
    public string? Uuid { get; set; }
    public float? Odometer { get; set; }
    public string? ActivityType { get; set; }
    public float? BatteryLevel { get; set; }
    public bool IsMoving { get; set; }
    public string UserId { get; set; } = string.Empty;
}

internal class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
{
    internal CreatePositionCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull().NotEmpty();
        RuleFor(x => x.Latitude).NotNull().NotEmpty();
        RuleFor(x => x.Longitude).NotNull().NotEmpty();

    }
}


public class CreatePositionCommandHandler(
    UserManager<Participant> participantRepository
    ) : IRequestHandler<CreatePositionCommand, Result<long?>>
{

    public async Task<Result<long?>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreatePositionCommandValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);


        var participant = await participantRepository.FindByIdAsync(request.UserId);
        participant = participant.ThrowIfNotFound(request.UserId);


        var entity = new Position(
            participant.Id,
            request.Latitude,
            request.Longitude,
            request.TimeStamp,
            request.IsMoving,
            request.Uuid,
            request.Speed,
            request.Heading,
            request.Altitude,
            request.ActivityType,
            request.BatteryLevel,
            request.Odometer);

        participant.AddPosition(entity);
        await participantRepository.UpdateAsync(participant);

        return Result<long?>.Success(entity.Id);
    }
}
