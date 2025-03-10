namespace ShareMyAdventures.Infrastructure.Adapters.Email;

public class SendGridOptions
{
    public const string Options = "SendGrid";
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
