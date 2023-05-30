namespace Pidp.Infrastructure.Services;

public class ScheduledPlrStatusChangeTaskHostService : BackgroundService
{
    private readonly ILogger<ScheduledPlrStatusChangeTaskHostService> logger;

    public ScheduledPlrStatusChangeTaskHostService(IServiceProvider services,
        ILogger<ScheduledPlrStatusChangeTaskHostService> logger)
    {
        this.Services = services;
        this.logger = logger;
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            this.logger.LogServiceIsRunning();

            await this.DoWork(stoppingToken);
        }
        catch (Exception e)
        {
            this.logger.LogServiceIsStoppedUnexpectedly(e);
        }
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        this.logger.LogServiceIsWorking();

        using (var scope = this.Services.CreateScope())
        {
            var scopedProcessingService =
                scope.ServiceProvider
                    .GetRequiredService<IScheduledPlrStatusChangeTaskService>();

            await scopedProcessingService.DoWorkAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogServiceIsStopping();

        await base.StopAsync(cancellationToken);
    }
}

public static partial class ScheduledPlrStatusChangeTaskHostServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Scoped Service Hosted Service for scheduled PLR status change running.")]
    public static partial void LogServiceIsRunning(this ILogger logger);
    [LoggerMessage(2, LogLevel.Information, "Scoped Service Hosted Service for scheduled PLR status change is working.")]
    public static partial void LogServiceIsWorking(this ILogger logger);
    [LoggerMessage(3, LogLevel.Information, "Scoped Service Hosted Service for scheduled PLR status change is stopping.")]
    public static partial void LogServiceIsStopping(this ILogger logger);
    [LoggerMessage(4, LogLevel.Critical, "Unhandled exception when scheduled PLR status change is running. Service stopped.")]
    public static partial void LogServiceIsStoppedUnexpectedly(this ILogger logger, Exception e);
}
