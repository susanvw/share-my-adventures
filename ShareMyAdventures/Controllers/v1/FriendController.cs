using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Friends.Commands;
using ShareMyAdventures.Application.UseCases.Friends.Queries; 

namespace ShareMyAdventures.Controllers.v1;

[Authorize]
public class FriendController : ApiControllerBase
{ 

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    public async Task<ActionResult<Result<PagedData<FriendView>?>>> List([FromQuery] ListWithPagingFriendsQuery query)
    {
        var response = await base.ExecuteAsync<ListWithPagingFriendsQuery, Result<PagedData<FriendView>?>>(query);
        return Ok(response);
    } 

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] long id)
    {
         await base.ExecuteAsync<DeleteFriendCommand, Unit>(new DeleteFriendCommand { Id = id});

        return NoContent();
    }

}
