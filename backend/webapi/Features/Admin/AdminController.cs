namespace Pidp.Features.Admin;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.IdirAuthentication, Roles = Roles.Admin)]
public class AdminController : PidpControllerBase
{
    public AdminController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpDelete("parties")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteParties([FromServices] ICommandHandler<PartyDelete.Command> handler)
    {
        if (PidpConfiguration.IsProduction())
        {
            return this.Forbid();
        }

        await handler.HandleAsync(new PartyDelete.Command());
        return this.NoContent();
    }

    [HttpGet("parties")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PartyIndex.Model>>> GetParties([FromServices] IQueryHandler<PartyIndex.Query, List<PartyIndex.Model>> handler,
                                                                       [FromQuery] PartyIndex.Query query)
        => await handler.HandleAsync(query);
}
