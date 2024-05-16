namespace Pidp.Features.Discovery;

using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Extensions;
using Pidp.Features.Credentials;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class DiscoveryController : PidpControllerBase
{
    public DiscoveryController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status307TemporaryRedirect)]
    public async Task<ActionResult<Discovery.Model>> Discovery([FromServices] IQueryHandler<Discovery.Query, Discovery.Model> handler)
    {
        var credentialLinkTicket = await this.AuthorizationService.VerifyTokenAsync<Cookies.CredentialLinkTicket.Values>(this.Request.Cookies.GetCredentialLinkTicket());
        if (credentialLinkTicket != null)
        {
            return this.RedirectToActionPreserveMethod
            (
                nameof(CredentialsController.CreateCredential),
                nameof(CredentialsController).Replace("Controller", "")
            );
        }

        return await handler.HandleAsync(new Discovery.Query { User = this.User });
    }

    [HttpGet("{partyId}/destination")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Destination.Model>> GetDestination([FromServices] IQueryHandler<Destination.Query, Destination.Model> handler,
                                                                      [FromRoute] Destination.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();
}
