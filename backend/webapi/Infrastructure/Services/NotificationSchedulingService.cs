namespace Pidp.Infrastructure.Services;
using Pidp.Infrastructure.HealthChecks;

public class NotificationSchedulingService : BackgroundService
{
    private readonly BackgroundWorkerHealthCheck healthCheck;
    private readonly ILogger<NotificationSchedulingService> logger;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly PeriodicTimer timer = new(TimeSpan.FromSeconds(10));
    public NotificationSchedulingService(
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationSchedulingService> logger,
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
                await scope.ServiceProvider.GetRequiredService<INotificationService>()
                    .SendEndorsementInactiveNotification(stoppingToken);
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
