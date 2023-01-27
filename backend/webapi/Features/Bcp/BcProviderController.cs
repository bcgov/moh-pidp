namespace Pidp.Features.Bcp;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/bcprovider")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class BcProviderController : PidpControllerBase
{
    public BcProviderController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> CreateUser([FromServices] ICommandHandler<Create.Command, string> handler,
                                                     [FromBody] Create.Command command)
        => await handler.HandleAsync(command);

}
