namespace Pidp.Features.Banners;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
public class BannersController(IPidpAuthorizationService authorizationService) : PidpControllerBase(authorizationService)
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Index.Model>>> GetActiveBanners([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                         [FromQuery] Index.Query query)
        => await handler.HandleAsync(query);

}
