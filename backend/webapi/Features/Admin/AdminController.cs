namespace Pidp.Features.Admin;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;

[Route("api/[controller]")]
[Authorize(Policy = Policies.IdirAuthentication, Roles = Roles.Admin)]
public class AdminController : PidpControllerBase
{
    public AdminController(IAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet("parties")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Index.Model>>> GetParties([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                  [FromQuery] Index.Query query)
        => await handler.HandleAsync(query);
}
