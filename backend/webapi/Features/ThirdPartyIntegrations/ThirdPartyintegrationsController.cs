using Microsoft.Graph.Education.Classes.Item.Assignments.Item.Submissions.Item.Return;

namespace Pidp.Features.ThirdPartyIntegrations;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/ext")]
public class ThirdPartyintegrationsController : PidpControllerBase
{
    public ThirdPartyintegrationsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet("parties/{hpdid}/endorsements")]
    [Authorize(Roles = Roles.ViewEndorsements)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<EndorsementData.Model>>> GetEndorsemetData([FromServices] IQueryHandler<EndorsementData.Query, IDomainResult<List<EndorsementData.Model>>> handler,
                                                                                   [FromRoute] EndorsementData.Query query)
        => await handler.HandleAsync(query)
            .ToActionResultOfT();

    [HttpPost("fhir/message")]
    [Authorize(Roles = Roles.ViewEndorsements)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PostFHIRMessage<T>(T obj)
    {
        Console.WriteLine(obj);
        return this.Ok();
    }
}