namespace Common.Adapter.Email;

/// <summary>
/// Represents configuration options for SendGrid email sending.
/// </summary>
public sealed record SendGridOptions
{
    /// <summary>
    /// The configuration section name for SendGrid settings.
    /// </summary>
    public const string Options = "SendGrid";

    /// <summary>
    /// Gets or sets the sender's email address.
    /// </summary>
    public string SenderEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sender's display name.
    /// </summary>
    public string SenderName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SendGrid API key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
}