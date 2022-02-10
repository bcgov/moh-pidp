namespace Pidp.Features.Lookups;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
public class LookupsController : PidpControllerBase
{
    public LookupsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Index.Model>> GetLookups([FromServices] IQueryHandler<Index.Query, Index.Model> handler)
        => await handler.HandleAsync(new Index.Query());
}
