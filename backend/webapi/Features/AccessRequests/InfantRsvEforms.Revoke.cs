namespace Pidp.Features.AccessRequests;

using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public interface IInfantRsvEformsRevocationService
{
    /// <summary>
    /// Re-evaluates the Party's eligibility for the Infant RSV eForms enrolment and, if they are no
    /// longer eligible, removes the Keycloak role and deletes the Access Request.
    /// Changes are staged on the shared DbContext; the caller owns SaveChangesAsync.
    /// Does nothing if the Party does not hold the enrolment.
    /// </summary>
    Task RevokeIfIneligibleAsync(int partyId, PlrStatusChangeLog? statusChange = null, CancellationToken cancellationToken = default);
}

public class InfantRsvEformsRevocationService(
    IClock clock,
    IKeycloakAdministrationClient keycloakClient,
    ILogger<InfantRsvEformsRevocationService> logger,
    IPlrClient plrClient,
    PidpDbContext context) : IInfantRsvEformsRevocationService
{
    private readonly IClock clock = clock;
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly ILogger<InfantRsvEformsRevocationService> logger = logger;
    private readonly IPlrClient plrClient = plrClient;
    private readonly PidpDbContext context = context;

    public async Task RevokeIfIneligibleAsync(int partyId, PlrStatusChangeLog? statusChange = null, CancellationToken cancellationToken = default)
    {
        var dto = await this.context.Parties
            .Where(party => party.Id == partyId)
            .Select(party => new
            {
                AccessRequestId = party.AccessRequests
                    .Where(request => request.AccessTypeCode == AccessTypeCode.InfantRsvEforms)
                    .Select(request => (int?)request.Id)
                    .FirstOrDefault(),
                // Assign narrowly, remove broadly. The grant only ever targets BC Services Card
                // credentials, but removing from all of them costs nothing (Keycloak returns 204
                // for a role the User does not hold) and cleans up any grant made by an earlier
                // version of the rule.
                UserIds = party.Credentials
                    .Select(credential => credential.UserId)
                    .ToList(),
                party.Cpn,
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (dto?.AccessRequestId == null)
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

        foreach (var userId in dto.UserIds)
        {
            if (!await this.keycloakClient.RemoveAccessRoles(userId, MohKeycloakEnrolment.InfantRsvEforms))
            {
                // Leave the Access Request in place so the next status change or endorsement
                // update retries; deleting it would strand a live role with no record of it.
                this.logger.LogRoleRemovalFailed(partyId, userId);
                this.context.BusinessEvents.Add(AccessRequestRevoked.CreateFailure(
                    partyId,
                    AccessTypeCode.InfantRsvEforms,
                    MohKeycloakEnrolment.InfantRsvEforms,
                    dto.Cpn,
                    dto.UserIds,
                    reason,
                    FormatTrigger(statusChange),
                    this.clock.GetCurrentInstant()));
                return;
            }
        }

        // FindAsync returns the already-tracked instance when there is one, so this cannot collide
        // with an AccessRequest the caller has loaded.
        var accessRequest = await this.context.AccessRequests.FindAsync([dto.AccessRequestId.Value], cancellationToken);
        if (accessRequest != null)
        {
            this.context.AccessRequests.Remove(accessRequest);
        }

        this.context.BusinessEvents.Add(AccessRequestRevoked.Create(
            partyId,
            AccessTypeCode.InfantRsvEforms,
            MohKeycloakEnrolment.InfantRsvEforms,
            dto.Cpn,
            dto.UserIds,
            reason,
            FormatTrigger(statusChange),
            this.clock.GetCurrentInstant()));

        this.logger.LogAccessRevoked(partyId);
    }

    /// <summary>
    /// Describes what prompted the re-evaluation, for the audit record. The PLR status change is
    /// absent when an endorsement update triggered this rather than a licence status change.
    /// </summary>
    private static string? FormatTrigger(PlrStatusChangeLog? statusChange) => statusChange == null
        ? "endorsement standing updated"
        : $"PLR status {FormatStatus(statusChange.OldStatusCode, statusChange.OldStatusReasonCode)} -> {FormatStatus(statusChange.NewStatusCode, statusChange.NewStatusReasonCode)}";

    private static string FormatStatus(string? statusCode, string? statusReasonCode) => statusCode == null && statusReasonCode == null
        ? "unknown"
        : $"{statusCode}/{statusReasonCode}";

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
    [LoggerMessage(1, LogLevel.Information, "Infant RSV eForms access for Party {partyId} was revoked; they no longer meet the eligibility criteria.")]
    public static partial void LogAccessRevoked(this ILogger<InfantRsvEformsRevocationService> logger, int partyId);

    [LoggerMessage(2, LogLevel.Warning, "Could not determine PLR standing for Party {partyId}; Infant RSV eForms access was left in place.")]
    public static partial void LogRevocationSkippedPlrError(this ILogger<InfantRsvEformsRevocationService> logger, int partyId);

    [LoggerMessage(3, LogLevel.Error, "Failed to remove the Infant RSV eForms role from User {userId} of Party {partyId}; the Access Request was left in place to be retried.")]
    public static partial void LogRoleRemovalFailed(this ILogger<InfantRsvEformsRevocationService> logger, int partyId, Guid userId);
}
