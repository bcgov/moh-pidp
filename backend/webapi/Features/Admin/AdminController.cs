namespace Pidp.Features.Admin;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.IdirAuthentication, Roles = Roles.Admin)]
public class AdminController : ControllerBase
{
    [HttpGet("parties")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Index.Model>>> GetParties([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                  [FromQuery] Index.Query query)
        => await handler.HandleAsync(query);
}
