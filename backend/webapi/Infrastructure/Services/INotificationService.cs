namespace Pidp.Infrastructure.Services;

public interface INotificationService
{
    Task SendEndorsementInactiveNotification(CancellationToken stoppingToken);
}
