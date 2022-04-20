namespace Pidp.Infrastructure.Services;

using Pidp.Infrastructure.HttpClients.Mail;

public interface IEmailService
{
    Task SendAsync(Email email);
    Task<int> UpdateEmailLogStatuses(int limit);
}
