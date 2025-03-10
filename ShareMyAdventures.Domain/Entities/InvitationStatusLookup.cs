using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Domain.Entities;

public sealed class InvitationStatusLookup : IAggregateRoot
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
