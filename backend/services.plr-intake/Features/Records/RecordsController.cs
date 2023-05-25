namespace PlrIntake.Features.Records;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class RecordsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Index.Model>>> GetRecords([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                  [FromQuery] Index.Query query)
        => await handler.HandleAsync(query);

    [HttpGet("cpns")]
    public async Task<ActionResult<List<string>>> SearchCpns([FromServices] IQueryHandler<Search.Query, List<string>> handler,
                                                             [FromQuery] Search.Query query)
        => await handler.HandleAsync(query);

    [HttpGet("status-changes")]
    public async Task<ActionResult<List<StatusLog.Model>>> GetStatusChanges([FromServices] IQueryHandler<StatusLog.Query, List<StatusLog.Model>> handler)
        => await handler.HandleAsync(new StatusLog.Query());

    [HttpPut("status-changes/{statusChangeLogId}/processed")]
    public async Task<IActionResult> UpdateProcessed([FromServices] ICommandHandler<StatusLog.Command, IDomainResult> handler,
                                                     [FromRoute] StatusLog.Command command)
        => await handler.HandleAsync(command).ToActionResult();
}
