namespace Pidp.Infrastructure.Services;

using Pidp.Infrastructure.HealthChecks;

public class PlrStatusUpdateSchedulingService : BackgroundService
{
    private readonly BackgroundWorkerHealthCheck healthCheck;
    private readonly ILogger<PlrStatusUpdateSchedulingService> logger;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly PeriodicTimer timer = new(TimeSpan.FromSeconds(10));

    public PlrStatusUpdateSchedulingService(
        IServiceScopeFactory scopeFactory,
        ILogger<PlrStatusUpdateSchedulingService> logger,
        BackgroundWorkerHealthCheck healthCheck)
    {
        this.logger = logger;
        this.scopeFactory = scopeFactory;
        this.healthCheck = healthCheck;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogServiceIsExecuting();

        try
        {
            this.healthCheck.IsRunning = true;
            while (await this.timer.WaitForNextTickAsync(stoppingToken)
                && !stoppingToken.IsCancellationRequested)
            {
                using var scope = this.scopeFactory.CreateScope();
                await scope.ServiceProvider.GetRequiredService<IPlrStatusUpdateService>()
                    .DoWorkAsync(stoppingToken);
            }
        }
        catch (Exception e)
        {
            this.logger.LogServiceHasStoppedUnexpectedly(e);
        }
        finally
        {
            this.healthCheck.IsRunning = false;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogServiceIsStopping();

        await base.StopAsync(cancellationToken);
        this.healthCheck.IsRunning = false;
    }
}

public static partial class PlrStatusUpdateSchedulingServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "PLR Status Update Scheduling Service is executing.")]
    public static partial void LogServiceIsExecuting(this ILogger<PlrStatusUpdateSchedulingService> logger);

    [LoggerMessage(2, LogLevel.Information, "PLR Status Update Scheduling Service is stopping.")]
    public static partial void LogServiceIsStopping(this ILogger<PlrStatusUpdateSchedulingService> logger);

    [LoggerMessage(3, LogLevel.Critical, "Unhandled exception in PLR Status Update Scheduling Service. Service stopped.")]
    public static partial void LogServiceHasStoppedUnexpectedly(this ILogger<PlrStatusUpdateSchedulingService> logger, Exception e);
}
