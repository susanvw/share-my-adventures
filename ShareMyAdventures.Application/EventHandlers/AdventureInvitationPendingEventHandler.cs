using ShareMyAdventures.Domain.Events;

namespace ShareMyAdventures.Application.EventHandlers;

public class AdventureInvitationPendingEventHandler() : INotificationHandler<AdventureInvitationPendingEvent>
{
    //private readonly IHubService _hubContext = hubContext;

    public async Task Handle(AdventureInvitationPendingEvent @event, CancellationToken cancellationToken)
    {
       // await _hubContext.SendInviteNotificationAsync(@event.Item.Email, cancellationToken);

        // add to notification table
    }
}
