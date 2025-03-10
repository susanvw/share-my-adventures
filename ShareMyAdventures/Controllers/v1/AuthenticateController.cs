using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Authentication.Commands;

namespace ShareMyAdventures.Controllers.v1;

public class AuthenticateController : ApiControllerBase
{

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<AuthView?>>> Authenticate([FromBody] AuthenticateCommand command, CancellationToken cancellationToken = default)
    {
        var response = await ExecuteAsync< AuthenticateCommand, Result<AuthView?>>(command, cancellationToken);
        return Ok(response);
    }
}