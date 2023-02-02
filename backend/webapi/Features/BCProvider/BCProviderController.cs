namespace Pidp.Features.BCProvider;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/parties/{partyId:int}/credentials/bc-provider")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class BCProviderController : PidpControllerBase
{
    public BCProviderController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BCProviderDetails.Model>> BcProvider([FromServices] IQueryHandler<BCProviderDetails.Query, BCProviderDetails.Model> handler,
                                                                                        [FromRoute] BCProviderDetails.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BCProviderChangePassword([FromServices] ICommandHandler<BCProviderChangePassword.Command, IDomainResult> handler,
        [FromBody] BCProviderChangePassword.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();
}
