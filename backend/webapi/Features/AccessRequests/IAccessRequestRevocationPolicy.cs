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
    Task RevokeIfIneligibleAsync(int partyId, PlrStatusChangeLog? statusChange = null, CancellationToken cancellationToken = default);
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
