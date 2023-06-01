namespace Pidp.Infrastructure.Services;

public class PlrStatusUpdateSchedulingService : BackgroundService
{
    private readonly ILogger<PlrStatusUpdateSchedulingService> logger;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly PeriodicTimer timer = new(TimeSpan.FromSeconds(10));

    public PlrStatusUpdateSchedulingService(IServiceScopeFactory scopeFactory, ILogger<PlrStatusUpdateSchedulingService> logger)
    {
        this.logger = logger;
        this.scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogServiceIsExecuting();

        try
        {
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
