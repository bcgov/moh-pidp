namespace Pidp.Infrastructure.Services;

public interface IScheduledPlrStatusChangeTaskService : IDisposable
{
    void Start();
    Task StopAsync();
}
