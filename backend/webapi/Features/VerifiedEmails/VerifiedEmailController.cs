namespace Pidp.Features.VerifiedEmails;

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
    public async Task<ActionResult<Create.Model>> CreateVerifiedEmail([FromServices] ICommandHandler<Create.Command, Create.Model> handler,
                                                                      [FromHybrid][AutoValidateAlways] Create.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResultOfT();

    [HttpPost("verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Verify.Model>> VerifyEmail([FromServices] ICommandHandler<Verify.Command, IDomainResult<Verify.Model>> handler,
                                                              [FromHybrid][AutoValidateAlways] Verify.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResultOfT();
}
