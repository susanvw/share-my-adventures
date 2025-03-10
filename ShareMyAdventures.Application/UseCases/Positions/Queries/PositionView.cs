using ShareMyAdventures.Domain.Entities.ParticipantAggregate;

namespace ShareMyAdventures.Application.UseCases.Positions.Queries;

public sealed record PositionView
{
    public long Id { get; internal set; }
    public string UserId { get; internal set; } = string.Empty;
    public float Latitude { get; internal set; }
    public float Longitude { get; internal set; }
    public float? Speed { get; internal set; }
    public float? Heading { get; internal set; }
    public float? Altitude { get; internal set; }
    public string? TimeStamp { get; internal set; }
    public string? Uuid { get; internal set; }
    public string? Location { get; internal set; }
    public float? Odometer { get; internal set; }
    public string? ActivityType { get; internal set; }
    public float? BatteryLevel { get; internal set; }
    public bool IsMoving { get; internal set; }

    internal static PositionView MapFrom(Position entity)
    {
        return new PositionView
        {
            Id = entity.Id,
            ActivityType = entity.ActivityType
        };

    }
}
