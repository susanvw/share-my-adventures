namespace ShareMyAdventures.Application.Common.Interfaces;

public interface IHubService
{
    Task SendInviteNotificationAsync(string email, CancellationToken cancellationToken = default);
}
