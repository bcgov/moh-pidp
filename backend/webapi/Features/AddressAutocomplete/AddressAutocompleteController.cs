namespace Pidp.Features.AddressAutocomplete;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.AddressAutocomplete;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.BcscAuthentication)]
public class AddressAutocompleteController : ControllerBase
{
    [HttpGet("find")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<AddressAutocompleteFindResponse>>> FindAddresses([FromServices] IQueryHandler<Find.Query, List<AddressAutocompleteFindResponse>> handler,
                                                                                         [FromQuery] Find.Query query)
        => await handler.HandleAsync(query);

    [HttpGet("retrieve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<AddressAutocompleteRetrieveResponse>>> RetrieveAddresses([FromServices] IQueryHandler<Retrieve.Query, List<AddressAutocompleteRetrieveResponse>> handler,
                                                                                                 [FromQuery] Retrieve.Query query)
        => await handler.HandleAsync(query);
}
