using Microsoft.AspNetCore.Mvc;
using ShareMyAdventures.Filters;

namespace ShareMyAdventures.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiExceptionFilter]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;
    private ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected async Task<ActionResult<TResponse>> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null) return BadRequest("The request object cannot be null");
        var response = await Mediator.Send(request, cancellationToken);

        if (response is null) return NoContent();

        return (TResponse)response;
    }
}