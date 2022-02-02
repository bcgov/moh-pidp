namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.BcscAuthentication)]
public class AccessRequestsController : ControllerBase
{
    [HttpPost("sa-eforms")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateSAeFormsEnrolment([FromServices] ICommandHandler<SAeForms.Command, IDomainResult> handler,
                                                             [FromBody] SAeForms.Command command)
        => await handler.HandleAsync(command).ToActionResult();
}
