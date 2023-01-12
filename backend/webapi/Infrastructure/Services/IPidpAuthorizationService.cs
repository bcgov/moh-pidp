namespace Pidp.Infrastructure.Services;

using DomainResults.Common;
using System.Security.Claims;

public interface IPidpAuthorizationService
{
    /// <summary>
    /// Checks that the given Party both exists and can be accessed by the given User.
    /// </summary>
    /// <param name="partyId">The Id of the Party</param>
    /// <param name="user">The User to authorize against</param>
    /// <returns></returns>
    Task<IDomainResult> CheckPartyAccessibility(int partyId, ClaimsPrincipal user);
}
