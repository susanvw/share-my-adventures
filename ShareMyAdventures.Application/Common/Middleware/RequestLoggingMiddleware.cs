using Microsoft.AspNetCore.Http;

namespace ShareMyAdventures.Application.Common.Middleware;

public sealed class RequestLoggingMiddleware(
    RequestDelegate next, 
    ICurrentUser currentUserService, 
    IIdentityService identityService
    )
{
    public async Task Invoke(HttpContext context)
    {
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;
        var userId = currentUserService.UserId ?? string.Empty;
        string? userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            userName = await identityService.GetUserNameAsync(userId);
        }

        Serilog.Log.Information("Incoming Request: {Method} {Path} - User: {UserId} ({UserName})",
            requestMethod, requestPath, userId, userName);

        await next(context);
    }
}