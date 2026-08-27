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
    private readonly IAccessRequestRevocationService revocationService = revocationService;
    private readonly ILogger<SAEformsRevocationPolicy> logger = logger;
    private readonly IPlrClient plrClient = plrClient;
    private readonly PidpDbContext context = context;

    public AccessTypeCode AccessTypeCode => AccessTypeCode.SAEforms;

    public async Task RevokeIfIneligibleAsync(int partyId, PlrStatusChangeLog? statusChange = null, CancellationToken cancellationToken = default)
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
            return;
        }

        // Unlike every other eForm card, Special Authority has no endorsement path
        var partyPlrStanding = await this.plrClient.GetStandingsDigestAsync(dto.Cpn);

        // Fail closed: an unreachable PLR yields a digest with no records, which looks exactly like
        // "not in good standing". Revoking on that would strip every holder during an outage.
        if (partyPlrStanding.Error)
        {
            this.logger.LogRevocationSkippedPlrError(partyId);
            return;
        }

        if (SAEforms.IsEligible(partyPlrStanding))
        {
            return;
        }

        await this.revocationService.RevokeAsync(
            partyId,
            AccessTypeCode.SAEforms,
            "licence no longer in good standing and not a CPS postgraduate",
            statusChange.FormatTrigger(),
            cancellationToken);
    }
}

public static partial class SAEformsRevocationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Could not determine PLR standing for Party {partyId}; Special Authority eForms access was left in place.")]
    public static partial void LogRevocationSkippedPlrError(this ILogger<SAEformsRevocationPolicy> logger, int partyId);
}
