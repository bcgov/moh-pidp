namespace Pidp.Infrastructure.Services;

public interface IScheduledPlrStatusChangeTaskService : IDisposable
{
    Task DoWorkAsync(CancellationToken stoppingToken);
}
