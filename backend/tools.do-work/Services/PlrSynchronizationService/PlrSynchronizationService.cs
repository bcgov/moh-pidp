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
        this.logger.LogStartingPlrSynchronization();
        if (dryRun)
        {
            this.logger.LogDryRunMode();
        }
        var clientId = this.config.BCProviderClient.ClientId;

        var parties = await this.context.Parties
            .Include(party => party.Credentials)
            .Where(party => party.Cpn != null && party.Credentials.Any(c => c.IdentityProvider == IdentityProviders.BCProvider))
            .ToListAsync();

        this.logger.LogFoundParties(parties.Count);

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
            bcProviderAttributes.SetCollegeId(plrStanding.CollegeIds);

            foreach (var upn in upns)
            {
                if (upn == null) continue;

                var additionalData = bcProviderAttributes.AsAdditionalData();
                var currentAttributes = await this.bcProviderClient.GetUserAttributes(upn, additionalData.Keys.ToArray());

                var hasChanges = false;
                if (currentAttributes != null)
                {
                    foreach (var kvp in additionalData)
                    {
                        var newValueString = kvp.Value?.ToString()?.ToLowerInvariant() ?? "null";
                        var currentValueString = currentAttributes.TryGetValue(kvp.Key, out var currVal) ? (currVal?.ToString()?.ToLowerInvariant() ?? "null") : "null";

                        if (newValueString != currentValueString)
                        {
                            this.logger.LogAttributeChanging(upn, kvp.Key, currentValueString, newValueString);
                            hasChanges = true;
                        }
                    }
                }
                else
                {
                    this.logger.LogCouldNotRetrieveAttributes(upn);
                    hasChanges = true;
                }

                if (!hasChanges)
                {
                    this.logger.LogNoAttributeChangesRequired(upn);
                    continue;
                }

                this.logger.LogUpdatingEntraAttributes(upn);
                if (!dryRun)
                {
                    await this.bcProviderClient.UpdateAttributes(upn, additionalData);
                }
            }

            count++;
        }

        this.logger.LogFinishedSynchronizing(count);
    }
}

public static partial class PlrSynchronizationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Starting PLR to Entra synchronization...")]
    public static partial void LogStartingPlrSynchronization(this ILogger logger);

    [LoggerMessage(2, LogLevel.Information, "DRY RUN MODE: No updates will be applied to Entra.")]
    public static partial void LogDryRunMode(this ILogger logger);

    [LoggerMessage(3, LogLevel.Information, "Found {Count} parties with BCProvider credentials and a CPN.")]
    public static partial void LogFoundParties(this ILogger logger, int count);

    [LoggerMessage(4, LogLevel.Information, "UPN: {Upn} Attribute {Key} changing from {OldValue} to {NewValue}")]
    public static partial void LogAttributeChanging(this ILogger logger, string upn, string key, string oldValue, string newValue);

    [LoggerMessage(5, LogLevel.Warning, "UPN: {Upn} Could not retrieve current attributes from Entra.")]
    public static partial void LogCouldNotRetrieveAttributes(this ILogger logger, string upn);

    [LoggerMessage(6, LogLevel.Information, "UPN: {Upn} No attribute changes required.")]
    public static partial void LogNoAttributeChangesRequired(this ILogger logger, string upn);

    [LoggerMessage(7, LogLevel.Information, "Updating Entra attributes for UPN: {Upn}")]
    public static partial void LogUpdatingEntraAttributes(this ILogger logger, string upn);

    [LoggerMessage(8, LogLevel.Information, "Finished synchronizing {Count} parties.")]
    public static partial void LogFinishedSynchronizing(this ILogger logger, int count);
}
