using System.Net;

namespace Common.Adapter.Email;

public sealed record NotificationSentEvent
{
	public bool IsSuccessStatusCode { get; set; }
	public string Message { get; set; } = null!;
	public HttpStatusCode StatusCode { get; set; }
}
