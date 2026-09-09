namespace Pidp.Features.AccessRequests;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models.Lookups;

public class HcimWebPcrRevocationPolicy(
    IAccessRequestRevocationService revocationService,
    ILogger<HcimWebPcrRevocationPolicy> logger,
    IPlrClient plrClient,
    PidpDbContext context) : IAccessRequestRevocationPolicy
{
    private readonly IAccessRequestRevocationService revocationService = revocationService;
    private readonly ILogger<HcimWebPcrRevocationPolicy> logger = logger;
    private readonly IPlrClient plrClient = plrClient;
    private readonly PidpDbContext context = context;

    public AccessTypeCode AccessTypeCode => AccessTypeCode.HcimWebPcr;

    public async Task<RevocationDecision> RevokeIfIneligibleAsync(int partyId, PlrStatusChangeLog? statusChange = null, CancellationToken cancellationToken = default)
    {
        var dto = await this.context.Parties
            .Where(party => party.Id == partyId)
            .Select(party => new
            {
                HoldsEnrolment = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.HcimWebPcr),
                party.Cpn,
            })
            .SingleOrDefaultAsync(cancellationToken);

        // Bail out before any PLR call for the many Parties who never held this card.
        if (dto?.HoldsEnrolment != true)
        {
            return RevocationDecision.NotHeld(AccessTypeCode.HcimWebPcr);
        }

        var (eligible, standingIsKnown, reason) = await this.EvaluateEligibilityAsync(partyId, dto.Cpn);

        // Fail closed: an unreachable PLR yields a digest with no records, which looks exactly like
        // "not in good standing". Revoking on that would strip every holder during an outage.
        if (!standingIsKnown)
        {
            this.logger.LogRevocationSkippedPlrError(partyId);
            return RevocationDecision.StandingUnknown(AccessTypeCode.HcimWebPcr);
        }

        if (eligible)
        {
            return RevocationDecision.Eligible(AccessTypeCode.HcimWebPcr);
        }

        return await this.revocationService.RevokeAsync(partyId, AccessTypeCode.HcimWebPcr, reason, statusChange.FormatTrigger(), cancellationToken)
            ? RevocationDecision.Revoked(AccessTypeCode.HcimWebPcr, reason)
            : RevocationDecision.RevokeFailed(AccessTypeCode.HcimWebPcr, reason);
    }

    /// <summary>
    /// Mirrors the grant-time checks in HcimWebPcr.CommandHandler: a Party with a CPN is judged
    /// on their own standing, one without a CPN (i.e. an MOA) on their endorsements.
    /// </summary>
    private async Task<(bool Eligible, bool StandingIsKnown, string Reason)> EvaluateEligibilityAsync(int partyId, string? cpn)
    {
        if (cpn == null)
        {
            var endorsementCpns = await this.context.ActiveEndorsementRelationships(partyId)
                .Select(relationship => relationship.Party!.Cpn)
                .ToListAsync();

            var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);

            return (HcimWebPcr.IsEligibleByEndorsement(endorsementPlrStanding),
                !endorsementPlrStanding.Error,
                "no active endorsement from a practitioner registered with an eligible college in good standing");
        }

        var partyPlrStanding = await this.plrClient.GetStandingsDigestAsync(cpn);

        return (HcimWebPcr.IsEligible(partyPlrStanding),
            !partyPlrStanding.Error,
            "licence no longer in good standing with an eligible college");
    }
}

public static partial class HcimWebPcrRevocationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Could not determine PLR standing for Party {partyId}; Provincial Client Registry access was left in place.")]
    public static partial void LogRevocationSkippedPlrError(this ILogger<HcimWebPcrRevocationPolicy> logger, int partyId);
}
