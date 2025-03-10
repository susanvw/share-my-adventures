using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

public sealed class FriendList: BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public List<Participant> Friends { get; set; } = [];
}
