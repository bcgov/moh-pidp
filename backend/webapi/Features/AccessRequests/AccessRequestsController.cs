namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;

[Route("api/[controller]")]
[Authorize(Policy = Policies.BcscAuthentication)]
public class AccessRequestsController : PidpControllerBase
{
    public AccessRequestsController(IAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpPost("sa-eforms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateSAEformsEnrolment([FromServices] ICommandHandler<SAEforms.Command, IDomainResult> handler,
                                                             [FromBody] SAEforms.Command command)
        => await handler.HandleAsync(command).ToActionResult();
}
