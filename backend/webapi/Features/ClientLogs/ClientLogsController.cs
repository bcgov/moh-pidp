namespace Pidp.Features.ClientLogs;

using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
public class ClientLogsController : PidpControllerBase
{
    public ClientLogsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateLog([FromServices] ICommandHandler<Create.Command> handler,
                                                 [FromBody] Create.Command command)
    => await handler.HandleAsync(command);


}
