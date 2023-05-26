namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;

public class ScheduledPlrStatusChangeTaskService : IScheduledPlrStatusChangeTaskService
{
    private Task? timerTask;
    private readonly PeriodicTimer timer;
    private readonly CancellationTokenSource cts = new();
    private readonly TimeSpan interval;
    private readonly IPlrClient plrClient;
    private readonly PidpDbContext context;
    private readonly IBCProviderClient bcProviderClient;

    public ScheduledPlrStatusChangeTaskService(
        IPlrClient plrClient,
        PidpDbContext context,
        IBCProviderClient bcProviderClient)
    {
        this.interval = TimeSpan.FromSeconds(10);
        this.timer = new PeriodicTimer(this.interval);
        this.plrClient = plrClient;
        this.context = context;
        this.bcProviderClient = bcProviderClient;
    }

    public void Start()
    {
        this.timerTask = this.DoWorkAsync();
    }

    private async Task DoWorkAsync()
    {
        try
        {
            while (await this.timer.WaitForNextTickAsync(this.cts.Token))
            {
                var statusChange = await this.plrClient.GetStatusChangeToPocess();
#if DEBUG
                Console.WriteLine($"{DateTime.Now} - {statusChange?.Count} Status change");
#endif
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
                            .SingleAsync();

                        if (!party.HasBCProviderCredential)
                        {
                            await this.plrClient.SetStatusChangeLogToProcessed(status.Id);
                        }
                        else
                        {
                            var plrStanding = await this.plrClient.GetStandingsDigestAsync(status.Cpn);

                            var userPrincipalName = await this.context.Credentials
                                .Where(credential => credential.PartyId == party.PartyId
                                    && credential.IdentityProvider == IdentityProviders.BCProvider)
                                .Select(credential => credential.IdpId)
                                .SingleOrDefaultAsync();

                            var isRnp = plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding;
                            var isMd = plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding;

                            // Update all people this Cpn holder has an endorsement relationship
                            var endorsementCpns = await this.context.ActiveEndorsementRelationships(party.PartyId)
                                .Select(relationship => relationship.Party!.Cpn)
                                .ToListAsync();

                            foreach (var endorsementCpn in endorsementCpns)
                            {
                                var endorseePlrStanding = await this.plrClient.GetStandingsDigestAsync(endorsementCpn);
                                var endorseeIsMoa = endorseePlrStanding.HasGoodStanding;

                                var endorsee = await this.context.Parties
                                    .Where(party => party.Cpn == endorsementCpn)
                                    .Select(party => new
                                    {
                                        PartyId = party.Id,
                                        HasBCProviderCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider),
                                    })
                                    .SingleAsync();

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
                                        .SingleOrDefaultAsync();

                                    var endorseeBcProviderAttributes = new BCProviderAttributes(endorseeUserPrincipalName).SetIsMoa(endorseeIsMoa);
                                    await this.bcProviderClient.UpdateAttributes(endorseeUserPrincipalName, endorseeBcProviderAttributes.AsAdditionalData());
                                }
                            }

                            var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);
                            var isMoa = endorsementPlrStanding.HasGoodStanding;

                            var bcProviderAttributes = new BCProviderAttributes(userPrincipalName).SetIsRnp(isRnp).SetIsMd(isMd).SetIsMoa(isMoa);
                            await this.bcProviderClient.UpdateAttributes(userPrincipalName, bcProviderAttributes.AsAdditionalData());
                        }
#if DEBUG
                        Console.WriteLine($"cpn {status.Cpn}, status change from {status.OldStatusCode} to {status.NewStatusCode} - StatusId {status.Id}");
#endif

                        // update the status to "processed"
                        await this.plrClient.SetStatusChangeLogToProcessed(status.Id);
#if DEBUG
                        Console.WriteLine($"cpn {status.Cpn}, status processed");
#endif
                    }
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    public async Task StopAsync()
    {
        if (this.timerTask is null)
        {
            return;
        }

        this.cts.Cancel();
        await this.timerTask;
        this.cts.Dispose();
    }

    public void Dispose()
    {
        this.timer.Dispose();
        this.cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
