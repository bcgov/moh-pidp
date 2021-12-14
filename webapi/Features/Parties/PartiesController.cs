namespace Pidp.Features.Parties;

using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class PartiesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Index.Model>>> GetParties([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler)
        => await handler.HandleAsync(new Index.Query());

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateParty([FromServices] ICommandHandler<Create.Command, int> handler,
                                                     [FromBody] Create.Command command)
        => await handler.HandleAsync(command);
}
