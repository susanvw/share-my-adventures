using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities.ParticipantAggregate;

/// <summary>
/// Represents a notification sent to a participant in the ShareMyAdventures domain.
/// Managed through the <see cref="Participant"/> aggregate root.
/// </summary>
public sealed class Notification : BaseAuditableEntity
{
    /// <summary>
    /// Gets the message content of the notification.
    /// </summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the notification has been sent.
    /// </summary>
    public bool IsSent { get; private set; }

    /// <summary>
    /// Gets the ID of the participant receiving this notification.
    /// </summary>
    public string ParticipantId { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the participant receiving this notification.
    /// </summary>
    public Participant Participant { get; private set; } = null!;

    // EF Core parameterless constructor
    private Notification() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class.
    /// </summary>
    /// <param name="message">The message content of the notification.</param>
    /// <param name="participantId">The ID of the participant receiving the notification.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> or <paramref name="participantId"/> is null.</exception>
    public Notification(string message, string participantId)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));
        ParticipantId = participantId ?? throw new ArgumentNullException(nameof(participantId));
        IsSent = false; // Default to unsent
    }

    /// <summary>
    /// Marks the notification as sent.
    /// </summary>
    public void MarkAsSent()
    {
        IsSent = true;
    }
}