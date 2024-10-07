namespace Pidp.Features.Endorsements;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/parties/{partyId}/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class EndorsementsController(IPidpAuthorizationService authorizationService) : PidpControllerBase(authorizationService)
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Index.Model>>> GetEndorsements([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                       [FromRoute] Index.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpGet("ms-teams-privacy-officers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<MSTeamsPrivacyOfficers.Model>>> GetMSTeamsPrivacyOfficers([FromServices] IQueryHandler<MSTeamsPrivacyOfficers.Query, IDomainResult<List<MSTeamsPrivacyOfficers.Model>>> handler,
                                                                                                  [FromRoute] MSTeamsPrivacyOfficers.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPost("{endorsementId}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelEndorsement([FromServices] ICommandHandler<Cancel.Command, IDomainResult> handler,
                                                       [FromRoute] Cancel.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();
}
