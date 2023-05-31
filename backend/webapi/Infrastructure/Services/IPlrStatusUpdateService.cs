namespace Pidp.Infrastructure.Services;

public interface IPlrStatusUpdateService
{
    Task DoWorkAsync(CancellationToken stoppingToken);
}
