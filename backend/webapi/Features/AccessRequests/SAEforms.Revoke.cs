namespace Pidp.Features.AccessRequests;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models.Lookups;

public class SAEformsRevocationPolicy(
    IAccessRequestRevocationService revocationService,
    ILogger<SAEformsRevocationPolicy> logger,
    IPlrClient plrClient,
    PidpDbContext context) : IAccessRequestRevocationPolicy
{
    /// <summary>
    /// Special Authority has no endorsement path, so a Party with no CPN cannot qualify. Worth reporting
    /// separately from a lapsed licence: this population is data drift, not anyone losing their standing,
    /// and a reviewer seeing a lot of it should investigate rather than approve.
    /// </summary>
    private const string NoCpnReason = "no CPN on record; Special Authority eForms is granted on the Party's own licence and has no endorsement path";

    private const string NotInGoodStandingReason = "licence no longer in good standing and not a CPS postgraduate";

    private readonly IAccessRequestRevocationService revocationService = revocationService;
    private readonly ILogger<SAEformsRevocationPolicy> logger = logger;
    private readonly IPlrClient plrClient = plrClient;
    private readonly PidpDbContext context = context;

    public AccessTypeCode AccessTypeCode => AccessTypeCode.SAEforms;

    public async Task<RevocationDecision> RevokeIfIneligibleAsync(int partyId, PlrStatusChangeLog? statusChange = null, CancellationToken cancellationToken = default)
    {
        var dto = await this.context.Parties
            .Where(party => party.Id == partyId)
            .Select(party => new
            {
                HoldsEnrolment = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.SAEforms),
                party.Cpn,
            })
            .SingleOrDefaultAsync(cancellationToken);

        // Bail out before any PLR call for the many Parties who never held this card.
        if (dto?.HoldsEnrolment != true)
        {
            return RevocationDecision.NotHeld(AccessTypeCode.SAEforms);
        }

        // Matches the grant-time check in SAEforms.CommandHandler, which asks PLR about a null CPN and
        // gets an empty digest back. Deciding it here instead spares the round trip and, more usefully,
        // lets the outcome say what actually disqualified them.
        if (dto.Cpn == null)
        {
            return await this.RevokeAsync(partyId, NoCpnReason, statusChange, cancellationToken);
        }

        var partyPlrStanding = await this.plrClient.GetStandingsDigestAsync(dto.Cpn);

        // Fail closed: an unreachable PLR yields a digest with no records, which looks exactly like
        // "not in good standing". Revoking on that would strip every holder during an outage.
        if (partyPlrStanding.Error)
        {
            this.logger.LogRevocationSkippedPlrError(partyId);
            return RevocationDecision.StandingUnknown(AccessTypeCode.SAEforms);
        }

        if (SAEforms.IsEligible(partyPlrStanding))
        {
            return RevocationDecision.Eligible(AccessTypeCode.SAEforms);
        }

        return await this.RevokeAsync(partyId, NotInGoodStandingReason, statusChange, cancellationToken);
    }

    private async Task<RevocationDecision> RevokeAsync(int partyId, string reason, PlrStatusChangeLog? statusChange, CancellationToken cancellationToken)
    {
        return await this.revocationService.RevokeAsync(partyId, AccessTypeCode.SAEforms, reason, statusChange.FormatTrigger(), cancellationToken)
            ? RevocationDecision.Revoked(AccessTypeCode.SAEforms, reason)
            : RevocationDecision.RevokeFailed(AccessTypeCode.SAEforms, reason);
    }
}

public static partial class SAEformsRevocationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Could not determine PLR standing for Party {partyId}; Special Authority eForms access was left in place.")]
    public static partial void LogRevocationSkippedPlrError(this ILogger<SAEformsRevocationPolicy> logger, int partyId);
}
