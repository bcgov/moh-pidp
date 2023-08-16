namespace Pidp.Features.Discovery;

using DomainResults.Common;
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> Discovery([FromServices] ICommandHandler<Discovery.Command, IDomainResult<int>> handler)
    {
        var result = await handler.HandleAsync(new Discovery.Command { User = this.User });
        if (result.IsSuccess)
        {
            return result.ToActionResultOfT();
        }

        if (this.Request.Cookies[Cookies.CredentialLinkToken.Key] is string credentialLinkTokenValue)
        {
            return this.RedirectToActionPreserveMethod
            (
                nameof(CredentialsController.CompleteCredentialLinkRequest),
                nameof(CredentialsController).Replace("Controller", ""),
                Cookies.CredentialLinkToken.DecodeValues(credentialLinkTokenValue)
            );
        }

        if (this.User.GetIdentityProvider() == IdentityProviders.BCProvider)
        {
            // BC Provider Users cannot create Parties (they can only link).
            return this.Conflict();
        }

        return this.NotFound();
    }
}
