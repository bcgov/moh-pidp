namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Pidp.Data;
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
    private readonly IBCProviderClient bCProviderClient;

    public ScheduledPlrStatusChangeTaskService(
        IPlrClient plrClient,
        PidpDbContext context,
        IBCProviderClient bCProviderClient)
    {
        this.interval = TimeSpan.FromSeconds(10);
        this.timer = new PeriodicTimer(this.interval);
        this.plrClient = plrClient;
        this.context = context;
        this.bCProviderClient = bCProviderClient;
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
                                HasBCProviderCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider),
                            })
                            .SingleAsync();

                        if (!party.HasBCProviderCredential)
                        {
                            return;
                        }

                        var plrStanding = await this.plrClient.GetStandingsDigestAsync(status.Cpn);
                        // perform business rules
#if DEBUG
                        Console.WriteLine($"cpn {status.Cpn}, status change from {status.OldStatusCode} to {status.NewStatusCode} - StatusId {status.Id}");
#endif

                        // update the status to "processed"
                        await this.plrClient.UpdateStatusChangeLog(status.Id);
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
