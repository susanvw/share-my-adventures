using Microsoft.AspNetCore.SignalR;
using System.Threading;

namespace ShareMyAdventures.Infrastructure.SignalR;

public sealed class HubService(IHubContext<NotificationHub> context) : IHubService
{
    public async Task SendInviteNotificationAsync(string email, CancellationToken cancellationToken = default)
    {
        await context.Clients.All.SendAsync("ReceiveNotification", email, "You are invited to a new adventure!", cancellationToken: cancellationToken);

    }
}
