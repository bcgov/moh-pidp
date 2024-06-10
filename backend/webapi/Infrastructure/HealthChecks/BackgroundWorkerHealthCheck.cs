namespace Pidp.Infrastructure.HealthChecks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

public class BackgroundWorkerHealthCheck : IHealthCheck
{
    public bool IsRunning { get; set; }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (this.IsRunning)
        {
            return Task.FromResult(HealthCheckResult.Healthy("The background service is running."));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("The background service has stopped."));
    }
}
