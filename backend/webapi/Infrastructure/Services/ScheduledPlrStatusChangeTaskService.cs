namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;

public class ScheduledPlrStatusChangeTaskService : IScheduledPlrStatusChangeTaskService
{
    private readonly PeriodicTimer timer = new(TimeSpan.FromSeconds(10));
    private readonly IPlrClient plrClient;
    private readonly PidpDbContext context;
    private readonly IBCProviderClient bcProviderClient;

    public ScheduledPlrStatusChangeTaskService(
        IPlrClient plrClient,
        PidpDbContext context,
        IBCProviderClient bcProviderClient)
    {
        this.plrClient = plrClient;
        this.context = context;
        this.bcProviderClient = bcProviderClient;
    }

    public async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (await this.timer.WaitForNextTickAsync(stoppingToken)
                && !stoppingToken.IsCancellationRequested)
            {
                var statusChange = await this.plrClient.GetStatusChangeToPocess();
                Console.WriteLine($"{DateTime.Now} - {statusChange?.Count} Status change");

                if (statusChange != null)
                {
                    foreach (var status in statusChange)
                    {
                        var party = await this.context.Parties
                            .Where(party => party.Cpn == status.Cpn)
                            .Select(party => new
                            {
                                PartyId = party.Id,
                                HasBCProviderCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider),
                            })
                            .SingleAsync(stoppingToken);

                        if (!party.HasBCProviderCredential)
                        {
                            await this.plrClient.SetStatusChangeLogToProcessed(status.Id);
                        }

                        var plrStanding = await this.plrClient.GetStandingsDigestAsync(status.Cpn);

                        var userPrincipalName = await this.context.Credentials
                            .Where(credential => credential.PartyId == party.PartyId
                                && credential.IdentityProvider == IdentityProviders.BCProvider)
                            .Select(credential => credential.IdpId)
                            .SingleOrDefaultAsync(stoppingToken);

                        var isRnp = plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding;
                        var isMd = plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding;



                        var endorsementCpns = await this.context.ActiveEndorsementRelationships(party.PartyId)
                            .Select(relationship => relationship.Party!.Cpn)
                            .ToListAsync(stoppingToken);

                        var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);
                        var isMoa = endorsementPlrStanding.HasGoodStanding;
                        var bcProviderAttributes = new BCProviderAttributes(userPrincipalName).SetIsRnp(isRnp).SetIsMd(isMd).SetIsMoa(isMoa);
                        await this.bcProviderClient.UpdateAttributes(userPrincipalName, bcProviderAttributes.AsAdditionalData());

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

                            if (!endorsee.HasBCProviderCredential)
                            {
                                await this.plrClient.SetStatusChangeLogToProcessed(status.Id);
                            }
                            else
                            {
                                var endorseeUserPrincipalName = await this.context.Credentials
                                    .Where(credential => credential.PartyId == endorsee.PartyId
                                        && credential.IdentityProvider == IdentityProviders.BCProvider)
                                    .Select(credential => credential.IdpId)
                                    .SingleOrDefaultAsync(stoppingToken);
                                var endorseeIsMoa = endorseePlrStanding.HasGoodStanding;
                                var endorseeBcProviderAttributes = new BCProviderAttributes(endorseeUserPrincipalName).SetIsMoa(endorseeIsMoa);
                                await this.bcProviderClient.UpdateAttributes(endorseeUserPrincipalName, endorseeBcProviderAttributes.AsAdditionalData());
                            }
                        }
                        Console.WriteLine($"cpn {status.Cpn}, status change from {status.OldStatusCode} to {status.NewStatusCode} - StatusId {status.Id}");

                        // update the status to "processed"
                        await this.plrClient.SetStatusChangeLogToProcessed(status.Id);
                        Console.WriteLine($"cpn {status.Cpn}, status processed");
                    }

                }
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    public void Dispose()
    {
        this.timer.Dispose();
        GC.SuppressFinalize(this);
    }
}
