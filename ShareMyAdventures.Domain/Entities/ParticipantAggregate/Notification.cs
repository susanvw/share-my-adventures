using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

public sealed class Notification: BaseAuditableEntity
{
    public string Message { get; set; } = string.Empty;
    public bool IsSent { get; set; } = false;

    public string ParticipantId { get; set; } = string.Empty;
    public Participant Participant { get; set; } = null!;
}
