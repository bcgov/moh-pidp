namespace Pidp.Features.AddressAutocomplete;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.AddressAutocomplete;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.AllowedPartyIdentityProvider)]
public class AddressAutocompleteController : PidpControllerBase
{
    public AddressAutocompleteController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<AddressAutocompleteFindResponse>>> FindAddresses([FromServices] IQueryHandler<Find.Query, List<AddressAutocompleteFindResponse>> handler,
                                                                                         [FromQuery] Find.Query query)
        => await handler.HandleAsync(query);

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<AddressAutocompleteRetrieveResponse>>> RetrieveAddresses([FromServices] IQueryHandler<Retrieve.Query, List<AddressAutocompleteRetrieveResponse>> handler,
                                                                                                 [FromRoute] Retrieve.Query query)
        => await handler.HandleAsync(query);
}
