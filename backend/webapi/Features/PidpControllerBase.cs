namespace Pidp.Features;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[ApiController]
public class PidpControllerBase : ControllerBase
{
    protected IAuthorizationService AuthorizationService { get; }

    protected PidpControllerBase(IAuthorizationService authService) => this.AuthorizationService = authService;

    protected Task UserMustOwnParty()
    {
        throw new NotImplementedException();
    }
}
