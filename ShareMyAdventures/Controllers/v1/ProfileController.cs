using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Profiles.Commands;
using ShareMyAdventures.Application.UseCases.Profiles.Queries;

namespace ShareMyAdventures.Controllers.v1;

[Authorize]
public class ProfileController : ApiControllerBase
{

    [HttpPatch("[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> FollowMe(CancellationToken cancellationToken = default)
    {
        await ExecuteAsync<ToggleTrackingCommand, Unit>(new ToggleTrackingCommand(), cancellationToken);
        return NoContent();
    }


    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<ProfileView?>>> Get([FromRoute] string userId, CancellationToken cancellationToken = default)
    {
        var query = new GetProfileByUserIdQuery { UserId = userId };

        var response = await ExecuteAsync<GetProfileByUserIdQuery, Result<ProfileView?>>(query, cancellationToken);
        return Ok(response);
    }


    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put([FromRoute] string userId, [FromBody] UpdateProfileCommand command, CancellationToken cancellationToken = default)
    {
        if (userId != command.UserId)
        {
            return BadRequest("The User Id provided is not valid.");
        }

        await ExecuteAsync<UpdateProfileCommand, Unit>(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{userId}/[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Patch([FromRoute] string userId, [FromBody] UploadAvatarCommand command, CancellationToken cancellationToken = default)
    {
        if (userId != command.UserId)
        {
            return BadRequest("The User Id provided is not valid.");
        }

        await ExecuteAsync<UploadAvatarCommand, Unit>(command, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{userId}/[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ToggleTracking([FromRoute] string userId, [FromBody] ToggleTrackingCommand command, CancellationToken cancellationToken = default)
    {
        if (userId != command.UserId)
        {
            return BadRequest("The User Id provided is not valid.");
        }

        await ExecuteAsync<ToggleTrackingCommand, Unit>(command, cancellationToken);
        return NoContent();
    }
}
