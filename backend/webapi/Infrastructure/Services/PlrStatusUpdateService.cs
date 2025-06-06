namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.DomainEvents;

public sealed class PlrStatusUpdateService(
    IBCProviderClient bcProviderClient,
    IPlrClient plrClient,
    ILogger<PlrStatusUpdateService> logger,
    PidpDbContext context,
    PidpConfiguration config) : IPlrStatusUpdateService
{
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
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

            foreach (var upn in party.Upns)
            {
                await this.bcProviderClient.UpdateAttributes(upn, bcProviderAttributes.AsAdditionalData());
            }
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
