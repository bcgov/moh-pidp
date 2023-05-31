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
        var statusChanges = await this.plrClient.GetProcessableStatusChangesAsync(1);
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
                UserPrincipalName = party.Credentials
                    .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                    .Select(credential => credential.IdpId)
                    .SingleOrDefault(),
            })
            .SingleOrDefaultAsync(stoppingToken);

        if (party == null)
        {
            // Status update is for a PLR record not associated to a PidP user.
            // TODO: Log
            await this.plrClient.UpdateStatusChangeLogAsync(status.Id);
            return;
        }

        var endorsementCpns = await this.context.ActiveEndorsementRelationships(party.PartyId)
            .Select(relationship => relationship.Party!.Cpn)
            .ToListAsync(stoppingToken);

        if (party.UserPrincipalName != null)
        {
            var isMd = status.ProviderRoleType == ProviderRoleType.MedicalDoctor && status.IsGoodStanding;
            var isRnp = status.ProviderRoleType == ProviderRoleType.RegisteredNursePractitioner && status.IsGoodStanding;

            var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);
            var isMoa = endorsementPlrStanding.HasGoodStanding;
            var bcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId).SetIsRnp(isRnp).SetIsMd(isMd).SetIsMoa(isMoa);
            await this.bcProviderClient.UpdateAttributes(party.UserPrincipalName, bcProviderAttributes.AsAdditionalData());
        }

        // Update all people this Cpn holder has an endorsement relationship
        foreach (var endorsementCpn in endorsementCpns)
        //TODO get rid of foreach with plr calls inside it
        {
            var endorsee = await this.context.Parties
                .Where(party => party.Cpn == endorsementCpn)
                .Select(party => new
                {
                    PartyId = party.Id,
                    UserPrincipalName = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.IdpId)
                        .SingleOrDefault(),
                })
                .SingleOrDefaultAsync(stoppingToken);

            if (endorsee == null)
            {
                // Status update is for a PLR record not associated to a PidP user.
                // TODO: Log
                await this.plrClient.UpdateStatusChangeLogAsync(status.Id);
                return;
            }

            if (endorsee.UserPrincipalName != null)
            {
                var endorseeCpns = await this.context.ActiveEndorsementRelationships(endorsee.PartyId)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync(stoppingToken);

                var endorseePlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorseeCpns);

                var endorseeIsMoa = endorseePlrStanding.HasGoodStanding;
                var endorseeBcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId).SetIsMoa(endorseeIsMoa);
                await this.bcProviderClient.UpdateAttributes(endorsee.UserPrincipalName, endorseeBcProviderAttributes.AsAdditionalData());
            }
        }

        await this.plrClient.UpdateStatusChangeLogAsync(status.Id);
    }
}

public static partial class ScheduledPlrStatusChangeTaskServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Unhandled exception when scheduled PLR status change is processing.")]
    public static partial void LogUnhandledExeption(this ILogger logger, Exception e);
}
