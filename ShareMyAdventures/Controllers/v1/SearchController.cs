
using ShareMyAdventures.Application.Common.Models;
using ShareMyAdventures.Application.UseCases.Search.Queries;

namespace ShareMyAdventures.Controllers.v1;

[Authorize]
public class SearchController : ApiControllerBase
{ 
 
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Result<PagedData<SearchView>?>>> Search([FromQuery] SearchQuery query)
    {
        var response = await base.ExecuteAsync<SearchQuery, Result<PagedData<SearchView>?>>(query);
        return Ok(response);
    }  
} 