namespace Pidp.Features.VerifiedEmail;

using DomainResults.Common;
using DomainResults.Mvc;
using HybridModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/parties/{partyId}/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class VerifiedEmailsController(IPidpAuthorizationService authorizationService) : PidpControllerBase(authorizationService)
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVerifiedEmail([FromServices] ICommandHandler<Create.Command> handler,
                                                         [FromHybrid][AutoValidateAlways] Create.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmail([FromServices] ICommandHandler<Verify.Command> handler,
                                                 [FromHybrid][AutoValidateAlways] Verify.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();
}
