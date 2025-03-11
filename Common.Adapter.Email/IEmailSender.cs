namespace Common.Adapter.Email;

public interface IEmailSender
{
    Task<NotificationSentEvent> SendHtmlAsync(string email, string subject, string message, CancellationToken cancellationToken = default);
    Task<NotificationSentEvent> SendTextAsync(string email, string subject, string message, CancellationToken cancellationToken = default);
}