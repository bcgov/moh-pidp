namespace Pidp.Features.Feedback;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
public class FeedbackController(IPidpAuthorizationService authorizationService) : PidpControllerBase(authorizationService)
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Create.Model>> Feedback([FromServices] ICommandHandler<Create.Command, Create.Model> handler,
                                                                        [FromForm] Create.Command command)
        => await handler.HandleAsync(command);
}
