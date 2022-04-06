namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Ldap;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
public class AccessRequestsController : PidpControllerBase
{
    public AccessRequestsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpPost("hcim-reenrolment")]
    [Authorize(Policy = Policies.PhsaAuthentication)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHcimReEnrolment([FromServices] ICommandHandler<HcimReEnrolment.Command, IDomainResult<HcimReEnrolment.Model>> handler,
                                                           [FromBody] HcimReEnrolment.Command command)
    {
        var access = await this.AuthorizationService.CheckPartyAccessibility(command.PartyId, this.User);
        if (!access.IsSuccess)
        {
            return access.ToActionResult();
        }

        var result = await handler.HandleAsync(command);
        if (!result.IsSuccess)
        {
            return result.ToActionResult();
        }

        switch (result.Value.AuthStatus)
        {
            case HcimLoginResult.AuthStatus.Success:
                return this.NoContent();
            case HcimLoginResult.AuthStatus.AccountLocked:
                return this.StatusCode(StatusCodes.Status423Locked);
            case HcimLoginResult.AuthStatus.AuthFailure:
                this.Response.SafeAddHeader("RemainingAttempts", result.Value.RemainingAttempts?.ToString(CultureInfo.InvariantCulture));
                return this.UnprocessableEntity();
            case HcimLoginResult.AuthStatus.Unauthorized:
                return this.Forbid();
            default:
                throw new NotImplementedException();
        }
    }

    [HttpPost("sa-eforms")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSAEformsEnrolment([FromServices] ICommandHandler<SAEforms.Command, IDomainResult> handler,
                                                             [FromBody] SAEforms.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();
}
