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
            var isMd = status.ProviderRoleType == ProviderRoleType.MedicalDoctor && status.IsGoodStanding;
            var isRnp = status.ProviderRoleType == ProviderRoleType.RegisteredNursePractitioner && status.IsGoodStanding;

            var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementRelations.Select(relation => relation.Cpn));
            var isMoa = endorsementPlrStanding.HasGoodStanding;
            var bcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId).SetIsRnp(isRnp).SetIsMd(isMd).SetIsMoa(isMoa);
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

            //TODO get rid of foreach with plr calls inside it
            var endorseePlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

            var endorseeIsMoa = endorseePlrStanding.HasGoodStanding;
            var endorseeBcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId).SetIsMoa(endorseeIsMoa);
            await this.bcProviderClient.UpdateAttributes(relation.UserPrincipalName, endorseeBcProviderAttributes.AsAdditionalData());
        }

        await this.plrClient.UpdateStatusChangeLogAsync(status.Id);
    }
}

public static partial class PlrStatusUpdateServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Status update {statusId} is for a PLR record not associated to a PidP user.")]
    public static partial void LogPlrRecordNotAssociatedToPidpUser(this ILogger logger, int statusId);
}
