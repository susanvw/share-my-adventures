using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ShareMyAdventures.Application.Common.Middleware;

public sealed class PerformanceMiddleware(
    RequestDelegate next, 
    ICurrentUser currentUserService,
    IIdentityService identityService
    )
{

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await next(context);
        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500) // Log if the request takes longer than 500ms
        {
            var requestPath = context.Request.Path;
            var userId = currentUserService.UserId ?? string.Empty;
            string? userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await identityService.GetUserNameAsync(userId);
            }

            Serilog.Log.Warning("Long Running Request: {Path} ({ElapsedMilliseconds}ms) User: {UserId} ({UserName})",
                requestPath, elapsedMilliseconds, userId, userName);
        }
    }
}
