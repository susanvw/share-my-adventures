namespace Common.Adapter.Email;

/// <summary>
/// Defines a contract for sending emails.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email with HTML content asynchronously.
    /// </summary>
    /// <param name="email">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The HTML content of the email.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, returning a <see cref="NotificationSentEvent"/>.</returns>
    Task<NotificationSentEvent> SendHtmlAsync(string email, string subject, string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an email with plain text content asynchronously.
    /// </summary>
    /// <param name="email">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The plain text content of the email.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, returning a <see cref="NotificationSentEvent"/>.</returns>
    Task<NotificationSentEvent> SendTextAsync(string email, string subject, string message, CancellationToken cancellationToken = default);
}