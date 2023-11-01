
namespace Pidp.Infrastructure.HealthChecks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

public class BackgroundWorkerHealthCheck : IHealthCheck
{
    private volatile bool isReady;

    public bool IsReady
    {
        get => this.isReady;
        set => this.isReady = value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (this.IsReady)
        {
            return Task.FromResult(HealthCheckResult.Healthy("The background service startup task has completed."));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("That background service startup task is still running."));
    }
}
