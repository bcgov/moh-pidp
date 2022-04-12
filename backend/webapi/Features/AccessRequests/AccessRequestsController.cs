namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
public class AccessRequestsController : PidpControllerBase
{
    public AccessRequestsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpPost("hcim-reenrolment")]
    [Authorize(Policy = Policies.HcimUser)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHcimReEnrolment([FromServices] ICommandHandler<HcimReEnrolment.Command, IDomainResult> handler,
                                                           [FromBody] HcimReEnrolment.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("sa-eforms")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSAEformsEnrolment([FromServices] ICommandHandler<SAEforms.Command, IDomainResult> handler,
                                                             [FromBody] SAEforms.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();
}
