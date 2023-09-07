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
    Task<IDomainResult> CheckPartyAccessibilityAsync(int partyId, ClaimsPrincipal user);

    /// <summary>
    /// Signs a set of claims into a token.
    /// </summary>
    /// <param name="claims"></param>
    Task<string> SignTokenAsync(object claims);

    /// <summary>
    /// Verifys the signature in a signed token and returns the claims.
    /// Returns null if the token is null, the signature is invalid, the token is expired, or if the claims cannot be parsed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="token"></param>
    Task<T?> VerifyTokenAsync<T>(string? token) where T : class;
}
