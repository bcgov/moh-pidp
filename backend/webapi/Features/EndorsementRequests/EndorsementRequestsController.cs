namespace Pidp.Features.EndorsementRequests;

using DomainResults.Common;
using DomainResults.Mvc;
using HybridModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/Parties/{partyId}/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class EndorsementRequestsController : PidpControllerBase
{
    public EndorsementRequestsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Index.Model>>> GetEndorsementRequests([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                              [FromRoute] Index.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateEndorsementRequest([FromServices] ICommandHandler<Create.Command> handler,
                                                              [FromHybrid] Create.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpGet("received")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<ReceivedIndex.Model>>> GetReceivedEndorsementRequests([FromServices] IQueryHandler<ReceivedIndex.Query, List<ReceivedIndex.Model>> handler,
                                                                                              [FromRoute] ReceivedIndex.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPost("received")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReceiveEndorsementRequest([FromServices] ICommandHandler<Receive.Command, IDomainResult> handler,
                                                               [FromHybrid] Receive.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPut("received/{endorsementRequestId}/adjudicate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdjudicateEndorsementRequest([FromServices] ICommandHandler<Adjudicate.Command, IDomainResult> handler,
                                                                  [FromHybrid] Adjudicate.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();
}
