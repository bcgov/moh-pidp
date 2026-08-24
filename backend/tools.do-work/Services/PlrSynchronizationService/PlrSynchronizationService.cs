namespace DoWork.Services.PlrSynchronizationService;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Pidp;
using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;

public class PlrSynchronizationService(
    PidpDbContext context,
    IPlrClient plrClient,
    IBCProviderClient bcProviderClient,
    PidpConfiguration config,
    ILogger<PlrSynchronizationService> logger) : IPlrSynchronizationService
{
    private readonly PidpDbContext context = context;
    private readonly IPlrClient plrClient = plrClient;
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly PidpConfiguration config = config;
    private readonly ILogger<PlrSynchronizationService> logger = logger;

    public async Task SynchronizePlrToEntraAsync(bool dryRun)
    {
        this.logger.LogInformation("Starting PLR to Entra synchronization...");
        if (dryRun)
        {
            this.logger.LogInformation("DRY RUN MODE: No updates will be applied to Entra.");
        }
        var clientId = this.config.BCProviderClient.ClientId;

        var parties = await this.context.Parties
            .Include(party => party.Credentials)
            .Where(party => party.Cpn != null && party.Credentials.Any(c => c.IdentityProvider == IdentityProviders.BCProvider))
            .ToListAsync();

        this.logger.LogInformation("Found {Count} parties with BCProvider credentials and a CPN.", parties.Count);

        int count = 0;
        foreach (var party in parties)
        {
            var upns = party.Credentials
                .Where(c => c.IdentityProvider == IdentityProviders.BCProvider)
                .Select(c => c.IdpId)
                .ToList();

            if (upns.Count == 0)
            {
                continue;
            }

            var plrStanding = await this.plrClient.GetStandingsDigestAsync(party.Cpn);
            
            var endorsementRelations = await this.context.ActiveEndorsingParties(party.Id)
                .Select(p => p.Cpn)
                .ToListAsync();

            var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementRelations);

            var bcProviderAttributes = new BCProviderAttributes(clientId);
            
            bcProviderAttributes.SetIsMoa(!plrStanding.HasGoodStanding && endorsementPlrStanding.HasGoodStanding);
            bcProviderAttributes.SetIsMd(plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding);
            bcProviderAttributes.SetIsRnp(plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding);
            bcProviderAttributes.SetIsPharm(plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding);
            bcProviderAttributes.SetPractitionerRole(plrStanding.ProviderRoleTypes);
            bcProviderAttributes.SetMspId(plrStanding.MspIds);

            foreach (var upn in upns)
            {
                if (upn == null) continue;
                this.logger.LogInformation("Updating Entra attributes for UPN: {Upn}", upn);
                if (!dryRun)
                {
                    await this.bcProviderClient.UpdateAttributes(upn, bcProviderAttributes.AsAdditionalData());
                }
            }

            count++;
        }

        this.logger.LogInformation("Finished synchronizing {Count} parties.", count);
    }
}
