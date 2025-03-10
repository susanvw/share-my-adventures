using ShareMyAdventures.Application.UseCases.Tokens.Commands.RefreshToken;
using ShareMyAdventures.Application.UseCases.Tokens.Commands.RevokeToken;
using System.Net;
using System.Security.Claims;

namespace ShareMyAdventures.Controllers.v1;

public class TokenController : ApiControllerBase
{

    [ProducesResponseType(typeof(RefreshTokenResponse), (int)HttpStatusCode.OK)]
    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<RefreshTokenResponse>> Refresh([FromBody] RefreshTokenCommand command)
    {
        return await ExecuteAsync<RefreshTokenCommand, RefreshTokenResponse>(command);
    }
    [HttpPut, Authorize]
    [Route("[action]")]
    public async Task<ActionResult> Revoke()
    {
        var currentUserName = User.FindFirst(ClaimTypes.Email)?.Value;
        var command = new RevokeCommand { Username = currentUserName };
        await ExecuteAsync<RevokeCommand, Unit>(command);

        return NoContent();
    }
}
