namespace Pidp.Infrastructure.Services;

public class ConsumeScopedServiceHostedService : BackgroundService
{
    private readonly ILogger<ConsumeScopedServiceHostedService> logger;

    public ConsumeScopedServiceHostedService(IServiceProvider services,
        ILogger<ConsumeScopedServiceHostedService> logger)
    {
        this.Services = services;
        this.logger = logger;
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("Consume Scoped Service Hosted Service running.");

        await this.DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("Consume Scoped Service Hosted Service is working.");

        using (var scope = this.Services.CreateScope())
        {
            var scopedProcessingService =
                scope.ServiceProvider
                    .GetRequiredService<IScheduledPlrStatusChangeTaskService>();

            await scopedProcessingService.DoWorkAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
