namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;
using static Pidp.Infrastructure.HttpClients.Ldap.HcimAuthorizationStatus;

[Route("api/[controller]")]
public class AccessRequestsController : PidpControllerBase
{
    public AccessRequestsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet("{partyId}")]
    [Authorize(Policy = Policies.AnyPartyIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<Index.Model>>> GetAccessRequests([FromServices] IQueryHandler<Index.Query, IDomainResult<List<Index.Model>>> handler,
                                                                         [FromRoute] Index.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPost("hcim-reenrolment")]
    [Authorize(Policy = Policies.HcimUser)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status423Locked)]
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

        this.Response.SafeAddHeader("No-Retry", "true");

        switch (result.Value.AuthStatus)
        {
            case AuthorizationStatus.Authorized:
                return this.NoContent();
            case AuthorizationStatus.AccountLocked:
                return this.StatusCode(StatusCodes.Status423Locked);
            case AuthorizationStatus.AuthFailure:
                this.Response.SafeAddHeader("Remaining-Attempts", result.Value.RemainingAttempts?.ToString(CultureInfo.InvariantCulture));
                return this.UnprocessableEntity();
            case AuthorizationStatus.Unauthorized:
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
