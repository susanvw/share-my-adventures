using ShareMyAdventures.Domain.Events;
using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

public sealed class Position : BaseAuditableEntity
{
    public string? TimeStamp { get; set; }
    public string? Event {  get; set; }
    public bool IsMoving { get; set; }
    public string? Uuid { get; set; }
    public long? Age { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? accuracy { get; set; }
    public double? Speed { get; set; }
    public double? Heading { get; set; }
    public double? Altitude { get; set; }
    public double? EllipsoidalAltitude { get; set; }
    public string? ActivityType { get; set; }
    public string? Confidence { get; set; }
    public double? BatteryLevel { get; set; }
    public bool IsCharging { get; set; }
    public double? Odometer { get; set; } // meters
    public string? Location { get;set; }

    public string ParticipantId { get; set; } = string.Empty;

    public Participant Participant { get; set; } = null!;

    public Position()
    {
        AddDomainEvent(new PositionEvent(this));
    }
}