
using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.AdventureInvitations.Commands;
using ShareMyAdventures.Application.UseCases.AdventureInvitations.Queries; 

namespace ShareMyAdventures.Controllers.v1;

[Authorize]
public class AdventureInvitationController : ApiControllerBase
{ 
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<PagedData<InvitationView>?>>> Get([FromQuery] ListWithPagingInvitationsByUserIdQuery query)
    {
        var response = await ExecuteAsync<ListWithPagingInvitationsByUserIdQuery, Result<PagedData<InvitationView>?>>(query);
        return Ok(response);
    }


    [HttpPatch("{adventureId:long}/[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Accept([FromRoute] long adventureId, [FromBody] AcceptInvitationCommand command)
    {
        if(command.AdventureId != adventureId)
        {
            return BadRequest("Adventure Id is not correct.");
        }
        await ExecuteAsync<AcceptInvitationCommand, Unit>(command);
        return NoContent();
    }


	[HttpPatch("{id:long}/[action]")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Reject([FromRoute] long id, [FromBody] RejectInvitationCommand command)
	{
		if (command.Id != id)
		{
			return BadRequest("Adventure Id is not correct.");
		}
		await ExecuteAsync<RejectInvitationCommand, Unit>(command);
        return NoContent();
    }
}
