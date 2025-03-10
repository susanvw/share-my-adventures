using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Friends.Commands;
using ShareMyAdventures.Application.UseCases.Friends.Queries;


namespace ShareMyAdventures.Controllers.v1;


[Authorize]
public class FriendRequestController : ApiControllerBase
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<PagedData<FriendRequestView>?>>> List([FromQuery] ListWithPagingFriendRequestsQuery query)
    {
        var response = await base.ExecuteAsync<ListWithPagingFriendRequestsQuery, Result<PagedData<FriendRequestView>?>>(query);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<long?>>> FriendRequest([FromBody] SendInviteCommand command, CancellationToken cancellationToken = default)
    {
        var response = await base.ExecuteAsync<SendInviteCommand, Result<long?>>(command, cancellationToken);

        return new CreatedResult("~", response);
    }


    [HttpPatch("{id}/[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Accept([FromRoute] long id)
    {
        var command = new UpdateFriendRequestCommand { Id = id, InvitationStatusLookupId = Domain.Enums.InvitationStatusLookups.Accepted.Id };
        await base.ExecuteAsync<UpdateFriendRequestCommand, Unit>(command);

        return NoContent();
    }


    [HttpPatch("{id}/[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Reject([FromRoute] long id)
    {
        var command = new UpdateFriendRequestCommand { Id = id, InvitationStatusLookupId = Domain.Enums.InvitationStatusLookups.Rejected.Id };
        await base.ExecuteAsync<UpdateFriendRequestCommand, Unit>(command);

        return NoContent();
    }
}