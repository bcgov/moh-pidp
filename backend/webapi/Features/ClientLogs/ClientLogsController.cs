namespace Pidp.Features.ClientLogs;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class ClientLogsController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task CreateLog([FromServices] ICommandHandler<Create.Command> handler,
                                                 [FromBody] Create.Command command)
        => await handler.HandleAsync(command);

}
