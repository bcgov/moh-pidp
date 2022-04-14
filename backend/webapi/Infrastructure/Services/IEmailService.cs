namespace Pidp.Infrastructure.Services;

using Pidp.Infrastructure.HttpClients.Mail;

public interface IEmailService
{
    Task SendAsync(Email email);
    Task SendSaEformsAccessRequestConfirmationAsync(int partyId);
    Task<int> UpdateEmailLogStatuses(int limit);
}
