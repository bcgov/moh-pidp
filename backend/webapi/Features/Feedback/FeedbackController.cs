namespace Pidp.Features.Feedback;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class FeedbackController(IPidpAuthorizationService authorizationService) : PidpControllerBase(authorizationService)
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateFeedback([FromServices] ICommandHandler<Create.Command> handler,
                                                    [FromForm] Create.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command);
}
