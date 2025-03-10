using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Accounts.Command;

namespace ShareMyAdventures.Controllers.v1;

public class AccountController : ApiControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<string?>>> Post([FromBody] RegisterAccountCommand command, CancellationToken cancellationToken = default)
    {
        var response = await ExecuteAsync<RegisterAccountCommand, Result<string?>>(command, cancellationToken);
        return CreatedAtAction("~", response);
    }


    [HttpPatch("[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Confirm([FromBody] ConfirmEmailCommand command, CancellationToken cancellationToken = default)
    {
        var response = await ExecuteAsync<ConfirmEmailCommand, Unit>(command, cancellationToken);
        return CreatedAtAction("~", response);
    }


    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] string userId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteAccountCommand { UserId = userId };
         await ExecuteAsync<DeleteAccountCommand, Unit>(command, cancellationToken);
        return NoContent();
    }
}