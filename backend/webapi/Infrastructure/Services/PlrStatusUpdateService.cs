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
            // TODO: handle error?
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
            this.logger.LogPlrRecordNotAssociatedToPidpUser(status.Id);
            await this.plrClient.UpdateStatusChangeLogAsync(status.Id);
            return;
        }

        var endorsementRelations = await this.context.ActiveEndorsementRelationships(party.PartyId)
            .Select(relationship => new
            {
                relationship.PartyId,
                relationship.Party!.Cpn,
                UserPrincipalName = relationship.Party!.Credentials
                    .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                    .Select(credential => credential.IdpId)
                    .SingleOrDefault()
            })
            .ToListAsync(stoppingToken);

        if (party.UserPrincipalName != null)
        {
            var bcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId);

            var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementRelations.Select(relation => relation.Cpn));
            // TODO: should probably remove this data if the current Party becomes licenced.
            var endorserData = endorsementPlrStanding
                .WithGoodStanding()
                .With(IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse, IdentifierType.Midwife)
                .Cpns;

            bcProviderAttributes.SetEndorserData(endorserData);

            if (!status.IsGoodStanding
                && !await this.plrClient.GetStandingAsync(status.Cpn))
            {
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


            await this.bcProviderClient.UpdateAttributes(party.UserPrincipalName, bcProviderAttributes.AsAdditionalData());
        }

        // Update the MOA flag for all people this Party has endorsed
        foreach (var relation in endorsementRelations)
        {
            if (!string.IsNullOrWhiteSpace(relation.Cpn)
                || string.IsNullOrWhiteSpace(relation.UserPrincipalName))
            {
                // We have nothing to update if the relation has a Licence or doesn't have a BC Provider credential
                continue;
            }

            var endorsingCpns = await this.context.ActiveEndorsementRelationships(relation.PartyId)
                .Select(relationship => relationship.Party!.Cpn)
                .ToListAsync(stoppingToken);

            var relationPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

            var relationIsMoa = relationPlrStanding.HasGoodStanding;
            var relationBcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId).SetIsMoa(relationIsMoa);
            await this.bcProviderClient.UpdateAttributes(relation.UserPrincipalName, relationBcProviderAttributes.AsAdditionalData());
        }

        this.logger.LogStatusUpdateProcessed(status.Id);
        await this.plrClient.UpdateStatusChangeLogAsync(status.Id);
    }
}

public static partial class PlrStatusUpdateServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Status update {statusId} is for a PLR record not associated to a PIdP user.")]
    public static partial void LogPlrRecordNotAssociatedToPidpUser(this ILogger logger, int statusId);

    [LoggerMessage(2, LogLevel.Information, "Status update {statusId} has been proccessed.")]
    public static partial void LogStatusUpdateProcessed(this ILogger logger, int statusId);
}
