namespace PlrIntake.Features.Records;

using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class RecordsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Index.Model>>> GetRecords([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                  [FromQuery] Index.Query query)
        => await handler.HandleAsync(query);

    // [HttpPost]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<int>> CreateParty([FromServices] ICommandHandler<Create.Command, int> handler,
    //                                                  [FromBody] Create.Command command)
    //     => await handler.HandleAsync(command);
}

