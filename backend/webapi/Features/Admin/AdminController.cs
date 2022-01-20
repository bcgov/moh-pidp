namespace Pidp.Features.Admin;

using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    [HttpGet("parties")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Index.Model>>> GetParties([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                  [FromQuery] Index.Query query)
        => await handler.HandleAsync(query);
}
