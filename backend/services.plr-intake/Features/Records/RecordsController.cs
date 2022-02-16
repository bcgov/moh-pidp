namespace PlrIntake.Features.Records;

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

    [HttpGet("{ipc}")]
    public async Task<ActionResult<Details.Model>> GetDetails([FromServices] IQueryHandler<Details.Query, Details.Model?> handler,
                                                              [FromRoute] Details.Query query)
    {
        var details = await handler.HandleAsync(query);
        if (details == null)
        {
            return this.NotFound();
        }
        return details;
    }
}
