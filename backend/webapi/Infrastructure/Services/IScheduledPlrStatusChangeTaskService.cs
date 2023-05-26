namespace Pidp.Infrastructure.Services;

public interface IScheduledPlrStatusChangeTaskService
{
    Task DoWorkAsync(CancellationToken stoppingToken);
}
