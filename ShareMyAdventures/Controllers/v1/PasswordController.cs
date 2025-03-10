using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Password.Commands;

namespace ShareMyAdventures.Controllers.v1;

public class PasswordController : ApiControllerBase
{

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Result<string?>>> Reminder([FromBody] SendPasswordReminderCommand command)
    {
        var response = await ExecuteAsync< SendPasswordReminderCommand, Result<string?>>(command);
        return new CreatedResult("~/", response);
    }
     
    [HttpPatch("[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Reset([FromBody] ResetPasswordCommand command)
    {
         await ExecuteAsync<ResetPasswordCommand, Unit>(command);
        return NoContent();
    }
}
