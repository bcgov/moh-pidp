namespace DoWork.Services.BulkEformRoles;

using Pidp.Infrastructure.Services;
using Pidp.Models.Lookups;

/// <summary>
/// The dry-run seam. Sits where the real revocation service would, and either delegates to it or does
/// nothing.
///
/// With no inner service (dry run) it records and does nothing: every IAccessRequestRevocationPolicy reads
/// from the DbContext but mutates nothing itself - the Keycloak removal, the Access Request delete and the
/// Business Event all happen inside IAccessRequestRevocationService - so replacing it removes every write
/// path while leaving the eligibility logic untouched. Returning true means "this would have gone ahead",
/// which is what the policy reports back to the caller.
///
/// With an inner service (apply) it records and delegates. Both modes therefore run the same policy the
/// same way; only the destination differs. That symmetry is deliberate - an apply path that took a
/// different route through the code would not be validated by the dry run that preceded it.
///
/// What each policy decided is read from the RevocationDecision it returns, not from here. This type only
/// has to answer whether a write happens.
/// </summary>
/// <param name="inner">The real service, or null for a dry run.</param>
public sealed class RecordingRevocationService(IAccessRequestRevocationService? inner = null) : IAccessRequestRevocationService
{
    private readonly IAccessRequestRevocationService? inner = inner;
    private readonly List<RecordedRevocation> recorded = [];

    public IReadOnlyList<RecordedRevocation> Recorded => this.recorded;

    public async Task<bool> RevokeAsync(int partyId, AccessTypeCode accessTypeCode, string reason, string? trigger = null, CancellationToken cancellationToken = default)
    {
        // In a dry run there is nothing to delegate to, and "true" means "it would have gone ahead".
        var applied = this.inner == null
            || await this.inner.RevokeAsync(partyId, accessTypeCode, reason, trigger, cancellationToken);

        this.recorded.Add(new RecordedRevocation(partyId, accessTypeCode, reason, trigger, applied));

        return applied;
    }

    /// <param name="Applied">
    /// False when the real service refused - a Keycloak removal that failed, say. The Access Request is
    /// deliberately left in place in that case so the next run retries it.
    /// </param>
    public sealed record RecordedRevocation(int PartyId, AccessTypeCode AccessTypeCode, string Reason, string? Trigger, bool Applied);
}
