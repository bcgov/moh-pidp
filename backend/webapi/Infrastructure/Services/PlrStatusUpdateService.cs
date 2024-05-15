namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.DomainEvents;

public sealed class PlrStatusUpdateService : IPlrStatusUpdateService
{
    private readonly IBCProviderClient bcProviderClient;
    private readonly IPlrClient plrClient;
    private readonly ILogger<PlrStatusUpdateService> logger;
    private readonly PidpDbContext context;
    private readonly string clientId;

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
        this.clientId = config.BCProviderClient.ClientId;
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
            .Include(party => party.Credentials)
            .Where(party => party.Cpn == status.Cpn)
            .SingleOrDefaultAsync(stoppingToken);

        if (party == null)
        {
            this.logger.LogPlrRecordNotAssociatedToPidpUser(status.Id);
            await this.plrClient.UpdateStatusChangeLogAsync(status.Id);
            return;
        }

        party.DomainEvents.Add(new CollegeLicenceUpdated(party.Id));

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

        var upn = party.Credentials
             .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
             .Select(credential => credential.IdpId)
             .SingleOrDefault();

        if (upn != null)
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

            await this.bcProviderClient.UpdateAttributes(upn, bcProviderAttributes.AsAdditionalData());
        }

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
}
