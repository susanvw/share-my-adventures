namespace ShareMyAdventures.Infrastructure.Adapters.Sms;

public class TwilioOptions
{
    public const string Options = "Twilio";
    public string AccountSid { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string FromNumber { get; set; } = string.Empty;
}
