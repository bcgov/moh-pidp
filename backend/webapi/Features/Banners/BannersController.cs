namespace Pidp.Features.Banners;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.Services;

[Route("api")]
public class BannersController : PidpControllerBase
{

    public BannersController(IPidpAuthorizationService authorizationService) : base(authorizationService) {
    }

    [HttpGet("banners")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Banners.Model>>> GetBannersData([FromServices] IQueryHandler<Banners.Query, List<Banners.Model>> handler,
                                                                         [FromRoute] Banners.Query query)
        =>  await handler.HandleAsync(query);

}
