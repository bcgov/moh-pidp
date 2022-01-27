namespace Pidp.Features.AddressAutocomplete;

using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.HttpClients.AddressAutocomplete;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class AddressAutocompleteController : ControllerBase
{
    [HttpGet("find")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AddressAutocompleteFindResponse>>> GetParties([FromServices] IQueryHandler<Find.Query, List<AddressAutocompleteFindResponse>> handler,
                                                                                      [FromQuery] Find.Query query)
        => await handler.HandleAsync(query);

    [HttpGet("retrieve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AddressAutocompleteRetrieveResponse>>> GetParties([FromServices] IQueryHandler<Retrieve.Query, List<AddressAutocompleteRetrieveResponse>> handler,
                                                                     [FromQuery] Retrieve.Query query)
        => await handler.HandleAsync(query);
}
