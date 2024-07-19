namespace Pidp.Features.HealthChecks;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Services;

[Route("/[controller]")]
public class HealthController(IPidpAuthorizationService authorizationService) : PidpControllerBase(authorizationService)
{
    [HttpGet("readiness")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Readiness([FromServices] IQueryHandler<Readiness.Query, IDomainResult> handler)
        => await handler.HandleAsync(new Readiness.Query())
            .ToActionResult();
}
