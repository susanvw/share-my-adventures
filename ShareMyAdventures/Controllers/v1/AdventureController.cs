using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Adventures.Commands;
using ShareMyAdventures.Application.UseCases.Adventures.Queries;
using ShareMyAdventures.Domain.Entities.AdventureAggregate;

namespace ShareMyAdventures.Controllers.v1;

[Authorize]
public class AdventureController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<long?>>> Post([FromBody] CreateAdventureCommand command, CancellationToken cancellationToken = default)
    {
        var response = await ExecuteAsync<CreateAdventureCommand, Result<long?>>(command, cancellationToken);
        return CreatedAtAction("~", response);
    }


    [HttpPatch("{id}/[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Start([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var command = new UpdateAdventureStatusCommand { Id = id, StatusLookupId = StatusLookup.InProgress.Id };
        await ExecuteAsync<UpdateAdventureStatusCommand, Unit>(command);
        return NoContent();
    }


    [HttpPatch("{id}/[action]")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Complete([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var command = new UpdateAdventureStatusCommand { Id = id, StatusLookupId = StatusLookup.Completed.Id };
        await ExecuteAsync<UpdateAdventureStatusCommand, Unit>(command);
        return NoContent();
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put([FromRoute] long id, [FromBody] UpdateAdventureCommand command, CancellationToken cancellationToken = default)
    {
        if (id != command.Id) return BadRequest("Id provided is not correct");

        await ExecuteAsync<UpdateAdventureCommand, Unit>(command);
        return NoContent();
    }



    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteAdventureCommand { Id = id };
        await ExecuteAsync<DeleteAdventureCommand, Unit>(command);
        return NoContent();
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<PagedData<AdventureView>?>>> ListWithPaging([FromQuery] ListWithPagingAdventuresByStatusIdQuery query, CancellationToken cancellationToken = default)
    {
        var response = await ExecuteAsync<ListWithPagingAdventuresByStatusIdQuery, Result<PagedData<AdventureView>?>>(query);
        return Ok(response);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<AdventureView?>>> Get([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        var response = await ExecuteAsync<GetAdventureByIdQuery, Result<AdventureView?>>(new GetAdventureByIdQuery { Id = id}, cancellationToken);
        return Ok(response);
    }
}