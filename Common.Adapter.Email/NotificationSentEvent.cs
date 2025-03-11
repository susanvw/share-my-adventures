using System.Net;

namespace Common.Adapter.Email;

/// <summary>
/// Represents the result of an email sending operation.
/// </summary>
public sealed record NotificationSentEvent
{
    /// <summary>
    /// Gets or sets a value indicating whether the email was sent successfully.
    /// </summary>
    public bool IsSuccessStatusCode { get; set; }

    /// <summary>
    /// Gets or sets the message describing the result of the operation.
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// Gets or sets the HTTP status code returned from the email sending operation.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }
}