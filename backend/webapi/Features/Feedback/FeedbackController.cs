namespace Pidp.Features.Feedback;

using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
public class FeedbackController(IPidpAuthorizationService authorizationService) : PidpControllerBase(authorizationService)
{
    [HttpPost]
    [Authorize(Policy = Policies.AnyPartyIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Create.Model>> CreateFeedback([FromServices] ICommandHandler<Create.Command, Create.Model> handler,
                                                           [FromForm] Create.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command).ToActionResultOfT();
}
