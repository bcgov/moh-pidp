namespace Pidp.Infrastructure.Services;

using Pidp.Infrastructure.HttpClients.Plr;

public class ScheduledPlrStatusChangeTaskService : IScheduledPlrStatusChangeTaskService
{
    private Task? timerTask;
    private readonly PeriodicTimer timer;
    private readonly CancellationTokenSource cts = new();
    private readonly TimeSpan interval;
    private readonly IPlrClient plrClient;

    public ScheduledPlrStatusChangeTaskService(IPlrClient plrClient)
    {
        this.interval = TimeSpan.FromSeconds(10);
        this.timer = new PeriodicTimer(this.interval);
        this.plrClient = plrClient;
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
                if (statusChange != null)
                {
                    foreach (var status in statusChange)
                    {
                        // perform business rules
                        Console.WriteLine($"Status change from {status.OldStatusCode} to {status.NewStatusCode} for the cpn {status.Cpn}");

                        // update the status to "processed"
                        await this.plrClient.UpdateStatusChangeLog(status.Id);
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
