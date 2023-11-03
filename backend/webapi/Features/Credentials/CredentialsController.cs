namespace Pidp.Features.Credentials;

using DomainResults.Common;
using DomainResults.Mvc;
using HybridModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;
using Pidp.Models;

[Route("api/parties/{partyId}/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class CredentialsController : PidpControllerBase
{
    public CredentialsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    /// <summary>
    /// Directly creating a new Credential on an existing Party requires a valid CredentialLinkTicket and associated cookie to link the Accounts.
    /// </summary>
    [HttpPost("/api/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> CreateCredential([FromServices] ICommandHandler<Create.Command, IDomainResult<int>> handler)
    {
        var credentialLinkTicket = await this.AuthorizationService.VerifyTokenAsync<Cookies.CredentialLinkTicket.Values>(this.Request.Cookies.GetCredentialLinkTicket());
        if (credentialLinkTicket == null)
        {
            return this.BadRequest("A valid Credential Link Ticket is required to link accounts.");
        }

        return await handler.HandleAsync(new Create.Command { CredentialLinkToken = credentialLinkTicket.CredentialLinkToken, User = this.User })
            .ToActionResultOfT();
    }

    [HttpGet("bc-provider")]
    [Authorize(Policy = Policies.HighAssuranceIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BCProviderDetails.Model>> GetBCProviderCredentialDetails([FromServices] IQueryHandler<BCProviderDetails.Query, IDomainResult<BCProviderDetails.Model>> handler,
                                                                                            [FromRoute] BCProviderDetails.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPost("bc-provider")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> CreateBCProviderCredential([FromServices] ICommandHandler<BCProviderCreate.Command, IDomainResult<string>> handler,
                                                                       [FromHybrid] BCProviderCreate.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResultOfT();

    [HttpPost("bc-provider/password")]
    [Authorize(Policy = Policies.HighAssuranceIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBCProviderPassword([FromServices] ICommandHandler<BCProviderPassword.Command, IDomainResult> handler,
                                                              [FromHybrid] BCProviderPassword.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("link-ticket")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateCredentialLinkTicket([FromServices] ICommandHandler<LinkTicketCreate.Command, IDomainResult<CredentialLinkTicket>> handler,
                                                                [FromHybrid] LinkTicketCreate.Command command)
    {
        var result = await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command);
        if (result.IsSuccess)
        {
            this.Response.Cookies.Append(
                Cookies.CredentialLinkTicket.Key,
                await this.AuthorizationService.SignTokenAsync(new Cookies.CredentialLinkTicket.Values(result.Value.Token)),
                new CookieOptions
                {
                    Expires = result.Value.ExpiresAt.ToDateTimeOffset(),
                    HttpOnly = true
                });

            return this.Ok();
        }

        return result.ToActionResult();
    }
}
