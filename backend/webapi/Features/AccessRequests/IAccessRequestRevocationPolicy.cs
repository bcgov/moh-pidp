namespace Pidp.Features.AccessRequests;

using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

/// <summary>
/// Decides whether a Party still qualifies for one particular Access Request, and revokes it if
/// not. One implementation per card: the eligibility rules are card-specific, while the mechanics
/// of revoking live in IAccessRequestRevocationService.
/// Implementations stage their changes on the shared DbContext; the caller owns SaveChangesAsync.
/// </summary>
public interface IAccessRequestRevocationPolicy
{
    AccessTypeCode AccessTypeCode { get; }

    /// <param name="partyId"></param>
    /// <param name="statusChange">The PLR status change that prompted this, when there was one.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// What was decided and why. Returned rather than inferred from the outside so a caller that needs to
    /// report on a decision - the bulk cleanup tool - reads it from the policy that made it. The webapi
    /// triggers ignore it.
    /// </returns>
    Task<RevocationDecision> RevokeIfIneligibleAsync(int partyId, PlrStatusChangeLog? statusChange = null, CancellationToken cancellationToken = default);
}

public enum RevocationOutcome
{
    /// <summary>The Party does not hold this Access Request, so there was nothing to evaluate.</summary>
    NotHeld,

    /// <summary>
    /// PLR standing could not be determined, so the access was deliberately left in place. Distinct from
    /// Eligible: this is "we do not know", not "they qualify".
    /// </summary>
    StandingUnknown,

    /// <summary>The Party still qualifies.</summary>
    Eligible,

    /// <summary>The Party no longer qualifies and the revocation went through.</summary>
    Revoked,

    /// <summary>
    /// The Party no longer qualifies but the revocation could not be completed - a failed Keycloak role
    /// removal, say. The Access Request is left in place so the next run retries it.
    /// </summary>
    RevokeFailed
}

/// <param name="Reason">Why this outcome was reached, in terms a reviewer reading a report can act on.</param>
public sealed record RevocationDecision(AccessTypeCode AccessTypeCode, RevocationOutcome Outcome, string Reason)
{
    /// <summary>True when the Party was found ineligible, whether or not the revocation succeeded.</summary>
    public bool WasIneligible => this.Outcome is RevocationOutcome.Revoked or RevocationOutcome.RevokeFailed;

    public static RevocationDecision NotHeld(AccessTypeCode accessTypeCode)
        => new(accessTypeCode, RevocationOutcome.NotHeld, "does not hold this Access Request");

    public static RevocationDecision StandingUnknown(AccessTypeCode accessTypeCode)
        => new(accessTypeCode, RevocationOutcome.StandingUnknown, "PLR standing could not be determined; access left in place");

    public static RevocationDecision Eligible(AccessTypeCode accessTypeCode)
        => new(accessTypeCode, RevocationOutcome.Eligible, "still meets the eligibility criteria");

    public static RevocationDecision Revoked(AccessTypeCode accessTypeCode, string reason)
        => new(accessTypeCode, RevocationOutcome.Revoked, reason);

    public static RevocationDecision RevokeFailed(AccessTypeCode accessTypeCode, string reason)
        => new(accessTypeCode, RevocationOutcome.RevokeFailed, reason);
}

public static class AccessRequestRevocationPolicyExtensions
{
    /// <summary>
    /// Describes what prompted a re-evaluation, for the audit record. The PLR status change is
    /// absent when an endorsement update triggered it rather than a licence status change.
    /// </summary>
    public static string FormatTrigger(this PlrStatusChangeLog? statusChange) => statusChange == null
        ? "endorsement standing updated"
        : $"PLR status {FormatStatus(statusChange.OldStatusCode, statusChange.OldStatusReasonCode)} -> {FormatStatus(statusChange.NewStatusCode, statusChange.NewStatusReasonCode)}";

    private static string FormatStatus(string? statusCode, string? statusReasonCode) => statusCode == null && statusReasonCode == null
        ? "unknown"
        : $"{statusCode}/{statusReasonCode}";
}
