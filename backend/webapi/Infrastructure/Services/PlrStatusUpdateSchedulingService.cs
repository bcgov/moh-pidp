namespace Pidp.Infrastructure.Services;

using Pidp.Infrastructure.HealthChecks;

public class PlrStatusUpdateSchedulingService : BackgroundService
{
    private readonly ILogger<PlrStatusUpdateSchedulingService> logger;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly PeriodicTimer timer = new(TimeSpan.FromSeconds(10));
    private readonly StartupHealthCheck healthCheck;

    public PlrStatusUpdateSchedulingService(IServiceScopeFactory scopeFactory, ILogger<PlrStatusUpdateSchedulingService> logger, StartupHealthCheck healthCheck)
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
            this.healthCheck.StartupCompleted = true;
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
            this.healthCheck.StartupCompleted = false;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogServiceIsStopping();

        await base.StopAsync(cancellationToken);
    }
}

public static partial class PlrStatusUpdateSchedulingServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "PLR Status Update Scheduling Service is executing.")]
    public static partial void LogServiceIsExecuting(this ILogger logger);

    [LoggerMessage(2, LogLevel.Information, "PLR Status Update Scheduling Service is stopping.")]
    public static partial void LogServiceIsStopping(this ILogger logger);

    [LoggerMessage(3, LogLevel.Critical, "Unhandled exception in PLR Status Update Scheduling Service. Service stopped.")]
    public static partial void LogServiceHasStoppedUnexpectedly(this ILogger logger, Exception e);
}
