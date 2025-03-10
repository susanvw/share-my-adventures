using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using ShareMyAdventures.Application.Common.Interfaces;

namespace ShareMyAdventures.Application.Common.Behaviors;

public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, ICurrentUser currentUserService, IIdentityService identityService) : IRequestPreProcessor<TRequest> where TRequest : notnull
{

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = currentUserService.UserId ?? string.Empty;
        string? userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            userName = await identityService.GetUserNameAsync(userId);
        }

        logger.LogInformation("SvwDesign Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}
