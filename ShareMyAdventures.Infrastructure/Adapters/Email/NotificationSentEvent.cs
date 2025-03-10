using System.Net;

namespace ShareMyAdventures.Application.Common.Models;

public sealed record NotificationSentEvent
{
	public bool IsSuccessStatusCode { get; set; }
	public string Message { get; set; } = string.Empty;
	public HttpStatusCode StatusCode { get; set; }
}
