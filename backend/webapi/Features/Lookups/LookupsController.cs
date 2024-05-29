namespace Pidp.Features.Lookups;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
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

    [HttpGet("test")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Index2.Model>> GetLookups2([FromServices] IQueryHandler<Index2.Query, Index2.Model> handler)
        => await handler.HandleAsync(new Index2.Query());

    [HttpHead("common-email-domains")]
    [Authorize(Policy = Policies.AnyPartyIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FindCommonEmailDomains([FromServices] IQueryHandler<CommonEmailDomains.Query, IDomainResult> handler,
                                                            [FromQuery] CommonEmailDomains.Query query)
        => await handler.HandleAsync(query)
            .ToActionResult();
}
