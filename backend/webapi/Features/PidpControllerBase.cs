namespace Pidp.Features;

using DomainResults.Common;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Services;
using Pidp.Models;

[Produces("application/json")]
[ApiController]
public class PidpControllerBase : ControllerBase
{
    protected IPidpAuthorizationService AuthorizationService { get; }

    protected PidpControllerBase(IPidpAuthorizationService authService) => this.AuthorizationService = authService;

    /// <summary>
    /// Checks that the given Party both exists and is owned by the current User.
    /// </summary>
    /// <param name="partyId"></param>
    /// <returns></returns>
    protected async Task<IDomainResult> CheckPartyAccessibility(int partyId)
    {
       return await this.AuthorizationService.CheckResourceAccessibility((Party p) => p.Id == partyId, this.User);
    }
}
