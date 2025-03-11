namespace Common.Adapter.Email;

public sealed record SendGridOptions
{
    public const string Options = "SendGrid";
    public string SenderEmail { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public string ApiKey { get; set; } = null!;
}
