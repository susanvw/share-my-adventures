using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Participants.Commands;
using ShareMyAdventures.Application.UseCases.Participants.Queries;
namespace ShareMyAdventures.Controllers.v1;

[Authorize]
public class ParticipantController : ApiControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<ParticipantView?>>> Get([FromRoute]string id)
    {
        var query = new GetParticipantQuery { Id = id };
        var response = await base.ExecuteAsync<GetParticipantQuery, Result<ParticipantView?>>(query);
        return Ok(response);
    }
    
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<PagedData<ParticipantView>?>>> List([FromQuery] ListWithPagingParticipantsQuery query)
    { 
        var response = await base.ExecuteAsync<ListWithPagingParticipantsQuery, Result<PagedData<ParticipantView>?>>(query);
        return Ok(response);

    }



    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InviteParticipantResponse>> Post([FromBody] InviteParticipantCommand command)
    {
        var response = await ExecuteAsync<InviteParticipantCommand, InviteParticipantResponse>(command);
        return new CreatedResult("~/", response);
    }
}
