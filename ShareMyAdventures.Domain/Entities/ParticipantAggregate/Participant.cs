using Microsoft.AspNetCore.Identity;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

public sealed class Participant : IdentityUser, IAggregateRoot
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public bool FollowMe { get; set; }
    public string? TrailColor { get; set; }
    public List<ParticipantAdventure> Adventures { get; set; } = [];
    public List<FriendRequest> Friends { get; set; } = [];
    public List<Position> Positions { get; set; } = [];
}
