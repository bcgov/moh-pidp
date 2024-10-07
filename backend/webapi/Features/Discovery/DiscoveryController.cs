namespace Pidp.Features.Discovery;

using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class DiscoveryController(IPidpAuthorizationService authorizationService) : PidpControllerBase(authorizationService)
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Discovery.Model>> PartyDiscovery([FromServices] IQueryHandler<Discovery.Query, Discovery.Model> handler)
    {
        var credentialLinkTicket = await this.AuthorizationService.VerifyTokenAsync<Cookies.CredentialLinkTicket.Values>(this.Request.Cookies.GetCredentialLinkTicket());

        var result = await handler.HandleAsync(new Discovery.Query { CredentialLinkToken = credentialLinkTicket?.CredentialLinkToken, User = this.User });

        if (result.Status is Discovery.Model.StatusCode.AlreadyLinked
            or Discovery.Model.StatusCode.CredentialExists
            or Discovery.Model.StatusCode.AccountLinkingError)
        {
            this.Response.Cookies.Append(
                Cookies.CredentialLinkTicket.Key,
                string.Empty,
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(-1),
                    HttpOnly = true
                });
        }

        return result;
    }

    [HttpGet("{partyId}/destination")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Destination.Model>> GetDestination([FromServices] IQueryHandler<Destination.Query, Destination.Model> handler,
                                                                      [FromRoute] Destination.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();
}
