namespace Pidp.Features.Feedback;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class FeedbackController(IPidpAuthorizationService authorizationService, ICommandHandler<Create.Command> commandHandler) : PidpControllerBase(authorizationService)
{
    private readonly ICommandHandler<Create.Command> commandHandler = commandHandler;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult
    > CreateFeedback([FromServices] ICommandHandler<Create.Command> handler,
                                     [FromForm] Create.Command command)
    {
        await this.commandHandler.HandleAsync(command);
        return this.Ok();
    }
    // => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command);
}
