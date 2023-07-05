namespace Pidp.Features.Credentials;

using DomainResults.Common;
using DomainResults.Mvc;
using HybridModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/parties/{partyId}/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class CredentialsController : PidpControllerBase
{
    public CredentialsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet("/api/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Index.Model>>> GetCredentials([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler)
        => await handler.HandleAsync(new Index.Query { User = this.User });

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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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
}
