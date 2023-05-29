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
        this.logger.ServiceIsRunning();

        await this.DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        this.logger.ServiceIsWorking();

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
        this.logger.ServiceIsStopping();

        await base.StopAsync(cancellationToken);
    }
}


public static partial class ScopedServiceHostedServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Scoped Service Hosted Service for scheduled PLR status change running.")]
    public static partial void ServiceIsRunning(this ILogger logger);
    [LoggerMessage(2, LogLevel.Information, "Scoped Service Hosted Service for scheduled PLR status change is working.")]
    public static partial void ServiceIsWorking(this ILogger logger);
    [LoggerMessage(3, LogLevel.Information, "Scoped Service Hosted Service for scheduled PLR status change is stopping.")]
    public static partial void ServiceIsStopping(this ILogger logger);
}
