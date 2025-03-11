using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Common.Adapter.Email;

/// <summary>
/// Implements email sending functionality using SendGrid.
/// </summary>
public class EmailSender : IEmailSender
{
    private readonly SendGridOptions _options;
    private readonly ISendGridClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSender"/> class.
    /// </summary>
    /// <param name="options">The SendGrid configuration options.</param>
    /// <param name="client">The SendGrid client for sending emails.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> or <paramref name="client"/> is null.</exception>
    public EmailSender(IOptions<SendGridOptions> options, ISendGridClient client)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(client);

        _options = options.Value ?? throw new ArgumentNullException(nameof(options), "SendGrid options value cannot be null.");
        _client = client;
    }

    /// <inheritdoc />
    public Task<NotificationSentEvent> SendHtmlAsync(string email, string subject, string message, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(subject, message, email, isHtml: true, cancellationToken);
    }

    /// <inheritdoc />
    public Task<NotificationSentEvent> SendTextAsync(string email, string subject, string message, CancellationToken cancellationToken = default)
    {
        return ExecuteAsync(subject, message, email, isHtml: false, cancellationToken);
    }

    /// <summary>
    /// Executes the email sending operation with the specified parameters.
    /// </summary>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The body of the email.</param>
    /// <param name="email">The recipient's email address.</param>
    /// <param name="isHtml">Indicates whether the message is HTML content.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation, returning a <see cref="NotificationSentEvent"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="email"/>, <paramref name="subject"/>, or <paramref name="message"/> is null.</exception>
    private async Task<NotificationSentEvent> ExecuteAsync(string subject, string message, string email, bool isHtml, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(subject);
        ArgumentNullException.ThrowIfNull(message);

        try
        {
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_options.SenderEmail, _options.SenderName),
                Subject = subject
            };

            if (isHtml)
            {
                msg.HtmlContent = message;
            }
            else
            {
                msg.PlainTextContent = message;
            }

            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);

            var response = await _client.SendEmailAsync(msg, cancellationToken).ConfigureAwait(false);

            return new NotificationSentEvent
            {
                StatusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                Message = await response.Body.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)
            };
        }
        catch (Exception ex)
        {
            return new NotificationSentEvent
            {
                IsSuccessStatusCode = false,
                Message = $"Failed to send email: {ex.Message}",
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}