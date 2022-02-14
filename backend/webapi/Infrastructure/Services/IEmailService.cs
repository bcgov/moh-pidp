namespace Pidp.Infrastructure.Services;

public interface IEmailService
{
    Task SendSaEformsAccessRequestConfirmationAsync(int partyId);
    Task<int> UpdateEmailLogStatuses(int limit);
}
