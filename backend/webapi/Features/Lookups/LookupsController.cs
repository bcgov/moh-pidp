namespace Pidp.Features.Lookups;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class LookupsController : PidpControllerBase
{
    public LookupsController(IAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Index.Model>> GetLookups([FromServices] IQueryHandler<Index.Query, Index.Model> handler)
        => await handler.HandleAsync(new Index.Query());
}
