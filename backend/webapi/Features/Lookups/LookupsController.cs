namespace Pidp.Features.Lookups;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class LookupsController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Index.Model>> GetLookups([FromServices] IQueryHandler<Index.Query, Index.Model> handler)
        => await handler.HandleAsync(new Index.Query());
}
