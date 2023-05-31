namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;

public sealed class PlrStatusUpdateService : IPlrStatusUpdateService
{
    private readonly IBCProviderClient bcProviderClient;
    private readonly IPlrClient plrClient;
    private readonly ILogger<PlrStatusUpdateService> logger;
    private readonly PidpConfiguration config;
    private readonly PidpDbContext context;

    public PlrStatusUpdateService(
        IBCProviderClient bcProviderClient,
        IPlrClient plrClient,
        ILogger<PlrStatusUpdateService> logger,
        PidpDbContext context,
        PidpConfiguration config)
    {
        this.bcProviderClient = bcProviderClient;
        this.plrClient = plrClient;
        this.logger = logger;
        this.context = context;
        this.config = config;
    }

    public async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        var statusChanges = await this.plrClient.GetProcessableStatusChanges(1);
        if (statusChanges == null)
        {
            // TODO: handle error
            return;
        }
        if (!statusChanges.Any())
        {
            return;
        }

        var status = statusChanges.Single();

        var party = await this.context.Parties
            .Where(party => party.Cpn == status.Cpn)
            .Select(party => new
            {
                PartyId = party.Id,
                HasBCProviderCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider),
            })
            .SingleOrDefaultAsync(stoppingToken);

        if (party == null)
        {
            // Unknow PidP user
            // TODO: Log
            await this.plrClient.SetStatusChangeLogToProcessed(status.Id);
            return;
        }

        var endorsementCpns = await this.context.ActiveEndorsementRelationships(party.PartyId)
            .Select(relationship => relationship.Party!.Cpn)
            .ToListAsync(stoppingToken);

        if (party.HasBCProviderCredential)
        {
            var userPrincipalName = await this.context.Credentials
                .Where(credential => credential.PartyId == party.PartyId
                    && credential.IdentityProvider == IdentityProviders.BCProvider)
                .Select(credential => credential.IdpId)
                .SingleOrDefaultAsync(stoppingToken);

            var isMd = status.ProviderRoleType == ProviderRoleType.MedicalDoctor && status.IsGoodStanding;
            var isRnp = status.ProviderRoleType == ProviderRoleType.RegisteredNursePractitioner && status.IsGoodStanding;

            var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);
            var isMoa = endorsementPlrStanding.HasGoodStanding;
            var bcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId).SetIsRnp(isRnp).SetIsMd(isMd).SetIsMoa(isMoa);
            await this.bcProviderClient.UpdateAttributes(userPrincipalName, bcProviderAttributes.AsAdditionalData());
        }

        // Update all people this Cpn holder has an endorsement relationship
        foreach (var endorsementCpn in endorsementCpns)
        {
            var endorseePlrStanding = await this.plrClient.GetStandingsDigestAsync(endorsementCpn);

            var endorsee = await this.context.Parties
                .Where(party => party.Cpn == endorsementCpn)
                .Select(party => new
                {
                    PartyId = party.Id,
                    HasBCProviderCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider),
                })
                .SingleAsync(stoppingToken);

            if (endorsee.HasBCProviderCredential)
            {
                var endorseeUserPrincipalName = await this.context.Credentials
                    .Where(credential => credential.PartyId == endorsee.PartyId
                        && credential.IdentityProvider == IdentityProviders.BCProvider)
                    .Select(credential => credential.IdpId)
                    .SingleOrDefaultAsync(stoppingToken);
                var endorseeIsMoa = endorseePlrStanding.HasGoodStanding;
                var endorseeBcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId).SetIsMoa(endorseeIsMoa);
                await this.bcProviderClient.UpdateAttributes(endorseeUserPrincipalName, endorseeBcProviderAttributes.AsAdditionalData());
            }
        }

        // update the status to "processed"
        await this.plrClient.SetStatusChangeLogToProcessed(status.Id);
    }
}

public static partial class ScheduledPlrStatusChangeTaskServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Unhandled exception when scheduled PLR status change is processing.")]
    public static partial void LogUnhandledExeption(this ILogger logger, Exception e);
}
