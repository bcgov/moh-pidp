
namespace Pidp.Infrastructure.HealthChecks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

public class StartupHealthCheck : IHealthCheck
{
    private volatile bool isReady;

    public bool StartupCompleted
    {
        get => this.isReady;
        set => this.isReady = value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (this.StartupCompleted)
        {
            return Task.FromResult(HealthCheckResult.Healthy("The background startup task has completed."));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("That background startup task is still running."));
    }
}
