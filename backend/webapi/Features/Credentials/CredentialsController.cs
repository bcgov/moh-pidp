namespace Pidp.Features.Credentials;

using DomainResults.Common;
using DomainResults.Mvc;
using HybridModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> CreateCredential([FromServices] ICommandHandler<Create.Command, IDomainResult<int>> handler,
                                                          [FromRoute] Create.Command command)
    {
        var cookieValues = Cookies.CredentialLinkTicket.DecodeValues(this.Request.Cookies[Cookies.CredentialLinkTicket.Key]);
        if (cookieValues == null)
        {
            return this.BadRequest("A Credential Link Ticket is required to link accounts.");
        }
        if (cookieValues.PartyId != command.PartyId)
        {
            return this.BadRequest("Party ID in cookie does not match route value.");
        }

        command.CredentialLinkToken = cookieValues.CredentialLinkToken;
        command.User = this.User;

        return await handler.HandleAsync(command)
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
                Cookies.CredentialLinkTicket.EncodeValues(command.PartyId, result.Value.Token),
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
