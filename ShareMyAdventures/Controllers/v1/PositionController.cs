using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Positions.Commands;
using ShareMyAdventures.Application.UseCases.Positions.Queries;

namespace ShareMyAdventures.Controllers.v1;

[Authorize]
public class PositionController : ApiControllerBase
{
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<long?>>> Create([FromBody] CreatePositionCommand command)
    {
        return await ExecuteAsync<CreatePositionCommand, Result<long?>>(command);
    }

    [HttpGet("[action]/{adventureId}")]
    public async Task<ActionResult<Result<IReadOnlyList<PositionView>?>>> LatestForAdventure([FromRoute] long adventureId, [FromQuery] DateTime fromDate)
    {
        var query = new GetLatestForAdventureQuery { AdventureId = adventureId,  FromDate = fromDate};
        return await ExecuteAsync<GetLatestForAdventureQuery, Result<IReadOnlyList<PositionView>?>>(query);
    }
}
