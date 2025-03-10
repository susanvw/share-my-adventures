using ShareMyAdventures.Domain.Entities.ParticipantAggregate;
using ShareMyAdventures.Domain.SeedWork;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Domain.Entities.AdventureAggregate;

public sealed class ParticipantAdventure : BaseAuditableEntity, IAggregateRoot
{
    public double Distance { get; set; }
    public string ParticipantId { get; set; } = string.Empty;
    public long AdventureId { get; set; }
    public int AccessLevelLookupId { get; set; }


    public Participant Participant { get; set; } = null!;
    public Adventure Adventure { get; set; } = null!;
    public AccessLevelLookup AccessLevelLookup { get; set; } = null!;
}
