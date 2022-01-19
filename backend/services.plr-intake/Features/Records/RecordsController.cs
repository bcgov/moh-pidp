namespace PlrIntake.Features.Records;

using Microsoft.AspNetCore.Mvc;

using PlrIntake.Data;
using PlrIntake.Models;

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

    [HttpPost]
    public async Task<ActionResult> CreateRecords([FromServices] PlrDbContext context)
    {
        context.PlrRecords.AddRange(new PlrRecord
        {
            Ipc = "1.CPSID",
            CollegeId = "11111",
            DateOfBirth = DateTime.Parse("2020-01-01 06:00:00"),
            IdentifierType = "CPSID"
        },
        new PlrRecord
        {
            Ipc = "1.CPSID-2",
            CollegeId = "11111",
            DateOfBirth = DateTime.Parse("2020-02-02 08:00:00"),
            IdentifierType = "CPSID"
        },
        new PlrRecord
        {
            Ipc = "1.RNID",
            CollegeId = "11111",
            DateOfBirth = DateTime.Parse("2020-01-01 06:00:00"),
            IdentifierType = "RNID"
        });

        await context.SaveChangesAsync();
        return Ok();
    }
}

