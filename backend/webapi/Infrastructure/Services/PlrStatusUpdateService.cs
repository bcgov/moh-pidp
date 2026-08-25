namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.DomainEvents;
using NodaTime;
using Pidp.Models.Lookups;

public sealed class PlrStatusUpdateService(
    IBCProviderClient bcProviderClient,
    IEnumerable<IAccessRequestRevocationPolicy> revocationPolicies,
    IClock clock,
    IPlrClient plrClient,
    ILogger<PlrStatusUpdateService> logger,
    PidpDbContext context,
    PidpConfiguration config) : IPlrStatusUpdateService
{
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly IEnumerable<IAccessRequestRevocationPolicy> revocationPolicies = revocationPolicies;
    private readonly IClock clock = clock;
    private readonly IPlrClient plrClient = plrClient;
    private readonly ILogger<PlrStatusUpdateService> logger = logger;
    private readonly PidpDbContext context = context;
    private readonly string clientId = config.BCProviderClient.ClientId;

    public async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        var statusChanges = await this.plrClient.GetProcessableStatusChangesAsync(1);
        if (statusChanges == null)
        {
            // TODO: handle error?
            return;
        }
        if (statusChanges.Count == 0)
        {
            return;
        }

        var status = statusChanges.Single();
        this.logger.LogProcessingStatusUpdateForCpn(status.Cpn);

        var party = await this.context.Parties
            .Include(party => party.Credentials)
            .Include(party => party.InvitedEntraAccounts)
            .Where(party => party.Cpn == status.Cpn)
            .SingleOrDefaultAsync(stoppingToken);

        if (party == null)
        {
            this.logger.LogPlrRecordNotAssociatedToPidpUser(status.Id);
            await this.plrClient.UpdateStatusChangeLogAsync(status.Id);
            return;
        }

        var endorsementRelations = await this.context.ActiveEndorsingParties(party.Id)
            .Select(party => new
            {
                PartyId = party.Id,
                party.Cpn,
            })
            .ToListAsync(stoppingToken);

        foreach (var relation in endorsementRelations)
        {
            party.DomainEvents.Add(new EndorsementStandingUpdated(relation.PartyId));
        }

        // Every update is now queued, we used to have '&& !status.IsGoodStandingUnchanged()'
        if (party.Upns.Any())
        {
            var bcProviderAttributes = new BCProviderAttributes(this.clientId);

            if (!status.IsGoodStanding
                && !await this.plrClient.GetStandingAsync(status.Cpn))
            {
                var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementRelations.Select(relation => relation.Cpn));
                bcProviderAttributes.SetIsMoa(endorsementPlrStanding.HasGoodStanding);
            }
            else
            {
                bcProviderAttributes.SetIsMoa(false);
            }

            if (status.ProviderRoleType == ProviderRoleType.MedicalDoctor)
            {
                bcProviderAttributes.SetIsMd(status.IsGoodStanding);
            }
            if (status.ProviderRoleType == ProviderRoleType.RegisteredNursePractitioner)
            {
                bcProviderAttributes.SetIsRnp(status.IsGoodStanding);
            }
            if (status.IdentifierType == IdentifierType.Pharmacist)
            {
                bcProviderAttributes.SetIsPharm(status.IsGoodStanding);
            }

            var plrStanding = await this.plrClient.GetStandingsDigestAsync(status.Cpn);
            bcProviderAttributes.SetPractitionerRole(plrStanding.ProviderRoleTypes);

            if (status.MspId != null)
            {
                bcProviderAttributes.SetMspId(plrStanding.MspIds);
            }

            foreach (var upn in party.Upns)
            {
                this.logger.LogApplyingAttributeUpdates(upn);
                await this.bcProviderClient.UpdateAttributes(upn, bcProviderAttributes.AsAdditionalData());
            }
        }

        // Runs on every status change: some cards' eligibility includes CPS postgrads, whose
        // transitions do not necessarily move the good-standing flag.
        foreach (var policy in this.revocationPolicies)
        {
            try
            {
                await policy.RevokeIfIneligibleAsync(party.Id, status, stoppingToken);
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                // A revocation that always threw would never be acked, blocking the head of the
                // queue and stalling every other consumer. Failing to revoke leaves today's
                // behaviour intact; stalling the pipeline does not. Policies stage nothing before
                // they can throw, and one failing card must not stop the others.
                this.logger.LogRevocationFailed(policy.AccessTypeCode, party.Id, e);
            }
        }
        this.context.BusinessEvents.Add(PlrStatusUpdated.Create(party.Id, status.ProviderRoleType ?? string.Empty, this.clock.GetCurrentInstant()));

        await this.context.SaveChangesAsync(stoppingToken);

        this.logger.LogStatusUpdateProcessed(status.Id);
        await this.plrClient.UpdateStatusChangeLogAsync(status.Id);
    }
}

public static partial class PlrStatusUpdateServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Status update {statusId} is for a PLR record not associated to a PIdP user.")]
    public static partial void LogPlrRecordNotAssociatedToPidpUser(this ILogger<PlrStatusUpdateService> logger, int statusId);

    [LoggerMessage(2, LogLevel.Information, "Status update {statusId} has been proccessed.")]
    public static partial void LogStatusUpdateProcessed(this ILogger<PlrStatusUpdateService> logger, int statusId);

    [LoggerMessage(3, LogLevel.Error, "Unhandled exception while re-evaluating {accessTypeCode} eligibility for Party {partyId}. The rest of the status update was processed.")]
    public static partial void LogRevocationFailed(this ILogger<PlrStatusUpdateService> logger, AccessTypeCode accessTypeCode, int partyId, Exception e);

    [LoggerMessage(4, LogLevel.Information, "Processing PLR status update for CPN {cpn}. Evaluating attributes for update.")]
    public static partial void LogProcessingStatusUpdateForCpn(this ILogger<PlrStatusUpdateService> logger, string? cpn);

    [LoggerMessage(5, LogLevel.Information, "Applying PLR-driven attribute updates to BC Provider account for UPN: {upn}")]
    public static partial void LogApplyingAttributeUpdates(this ILogger<PlrStatusUpdateService> logger, string upn);
}
