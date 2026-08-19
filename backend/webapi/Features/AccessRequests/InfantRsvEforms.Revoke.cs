namespace Pidp.Features.AccessRequests;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models.Lookups;

public class InfantRsvEformsRevocationPolicy(
    IAccessRequestRevocationService revocationService,
    ILogger<InfantRsvEformsRevocationPolicy> logger,
    IPlrClient plrClient,
    PidpDbContext context) : IAccessRequestRevocationPolicy
{
    private readonly IAccessRequestRevocationService revocationService = revocationService;
    private readonly ILogger<InfantRsvEformsRevocationPolicy> logger = logger;
    private readonly IPlrClient plrClient = plrClient;
    private readonly PidpDbContext context = context;

    public AccessTypeCode AccessTypeCode => AccessTypeCode.InfantRsvEforms;

    public async Task RevokeIfIneligibleAsync(int partyId, PlrStatusChangeLog? statusChange = null, CancellationToken cancellationToken = default)
    {
        var dto = await this.context.Parties
            .Where(party => party.Id == partyId)
            .Select(party => new
            {
                HoldsEnrolment = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.InfantRsvEforms),
                party.Cpn,
            })
            .SingleOrDefaultAsync(cancellationToken);

        // Bail out before any PLR call for the many Parties who never held this card.
        if (dto?.HoldsEnrolment != true)
        {
            return;
        }

        var (eligible, standingIsKnown, reason) = await this.EvaluateEligibilityAsync(partyId, dto.Cpn);

        // Fail closed: an unreachable PLR yields a digest with no records, which looks exactly like
        // "not in good standing". Revoking on that would strip every holder during an outage.
        if (!standingIsKnown)
        {
            this.logger.LogRevocationSkippedPlrError(partyId);
            return;
        }

        if (eligible)
        {
            return;
        }

        await this.revocationService.RevokeAsync(partyId, AccessTypeCode.InfantRsvEforms, reason, statusChange.FormatTrigger(), cancellationToken);
    }

    /// <summary>
    /// Mirrors the grant-time checks in InfantRsvEforms.CommandHandler: a Party with a CPN is judged
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

            return (InfantRsvEforms.IsEligibleByEndorsement(endorsementPlrStanding),
                !endorsementPlrStanding.Error,
                "no active endorsement from a Medical Doctor, Nurse, or Midwife in good standing");
        }

        var partyPlrStanding = await this.plrClient.GetStandingsDigestAsync(cpn);

        return (InfantRsvEforms.IsEligible(partyPlrStanding),
            !partyPlrStanding.Error,
            "licence no longer in good standing and not a CPS postgraduate");
    }
}

public static partial class InfantRsvEformsRevocationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Could not determine PLR standing for Party {partyId}; Infant RSV eForms access was left in place.")]
    public static partial void LogRevocationSkippedPlrError(this ILogger<InfantRsvEformsRevocationPolicy> logger, int partyId);
}
