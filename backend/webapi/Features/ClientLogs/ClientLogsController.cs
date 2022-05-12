namespace Pidp.Features.ClientLogs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class ClientLogsController
{
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateClientLog([FromServices] ICommandHandler<Create.Command> handler,
                                [FromBody] Create.Command command)
        => await handler.HandleAsync(command);
}
