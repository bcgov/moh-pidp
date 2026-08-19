namespace Pidp.Infrastructure.Services;

using Pidp.Models.Lookups;

public interface IAccessRequestRevocationService
{
    /// <summary>
    /// Removes the Keycloak roles associated with the given Access Type from every one of the
    /// Party's Credentials, deletes the Access Request, and records a Business Event.
    /// Knows nothing about eligibility - the caller decides whether a revocation is warranted.
    /// Changes are staged on the shared DbContext; the caller owns SaveChangesAsync.
    /// Returns false, having done nothing, if the Party does not hold the Access Request.
    /// </summary>
    /// <param name="partyId"></param>
    /// <param name="accessTypeCode"></param>
    /// <param name="reason">Why the Party is no longer entitled, recorded on the Business Event.</param>
    /// <param name="trigger">What prompted the re-evaluation, recorded on the Business Event.</param>
    /// <param name="cancellationToken"></param>
    Task<bool> RevokeAsync(int partyId, AccessTypeCode accessTypeCode, string reason, string? trigger = null, CancellationToken cancellationToken = default);
}
