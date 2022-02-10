namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.BcscAuthentication)]
public class AccessRequestsController : PidpControllerBase
{
    public AccessRequestsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpPost("sa-eforms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateSAEformsEnrolment([FromServices] ICommandHandler<SAEforms.Command, IDomainResult> handler,
                                                             [FromBody] SAEforms.Command command)
        => await this.CheckPartyAccessibility(command.PartyId)
            .IfSuccess(() => handler.HandleAsync(command))
            .ToActionResult();
}
