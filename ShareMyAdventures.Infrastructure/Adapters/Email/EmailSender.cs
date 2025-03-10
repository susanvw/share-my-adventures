using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using ShareMyAdventures.Application.Common.Extensions;
using ShareMyAdventures.Application.Common.Models;

namespace ShareMyAdventures.Infrastructure.Adapters.Email;

public class EmailSender(IOptions<SendGridOptions> options, ISendGridClient client) : IEmailSender
{
    private readonly SendGridOptions _options = options.Value;

    public async Task<NotificationSentEvent> SendHtmlAsync(string email, string subject, string message, CancellationToken cancellationToken = default)
    {
        return await Execute(subject, message, email, true);
    }


    public async Task<NotificationSentEvent> SendTextAsync(string email, string subject, string message, CancellationToken cancellationToken = default)
    {
        return await Execute(subject, message, email, false);
    }


    private async Task<NotificationSentEvent> Execute(string subject, string message, string email, bool isHtml, Dictionary<string, Stream>? streamAttachments = null!)
    {
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

            if (streamAttachments is not null)
            {
                foreach (var attachment in streamAttachments)
                {
                    await msg.AddAttachmentAsync(attachment.Key, attachment.Value);
                }
            }
            var addressList = new List<EmailAddress>
            {
                new(email)
            };
            msg.AddTos(addressList);

            msg.SetClickTracking(false, false);

            var response = await client.SendEmailAsync(msg);

            return new NotificationSentEvent
            {
                StatusCode = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                Message = await response.Body.ReadAsStringAsync()
            };
        }
        catch (Exception ex)
        {
            return new NotificationSentEvent
            {
                IsSuccessStatusCode = false,
                Message = ex.Message,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };

        }
    }
}
