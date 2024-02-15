namespace Pidp.Infrastructure.Services;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models;

public sealed class NotificationService : INotificationService
{
    private readonly PidpDbContext context;
    private readonly IEmailService emailService;
    private readonly IClock clock;
    private readonly PidpConfiguration config;

    public NotificationService(
        PidpDbContext context,
       IEmailService emailService,
        IClock clock,
        PidpConfiguration config
        )
    {
        this.context = context;
        this.emailService = emailService;
        this.clock = clock;
        this.config = config;
    }

    public async Task SendEndorsementInactiveNotification(CancellationToken stoppingToken)
    {
        var endorsements = await this.context.EndorsementRequests
                                    .Where(request =>
                                        (this.clock.GetCurrentInstant() - request.StatusDate).Days > 30 &&
                                        (request.Status != EndorsementRequestStatus.Cancelled || request.Status != EndorsementRequestStatus.Declined))
                                    .ToListAsync(stoppingToken);

        var oneHealthUrl = this.config.Notification.Url;

        var template = System.IO.File.ReadAllText("EmailTemplate/EndorsementInactiveNotification.html");

        foreach (var inactiveEndorsement in endorsements)
        {
            var templateBody = template.ToString();
            templateBody = templateBody.Replace("_OneHealthUrl", oneHealthUrl);//Replacing OneHealth Url

            var email = new Email(
                from: EmailService.PidpEmail,
                to: inactiveEndorsement.RecipientEmail,
                subject: $"OneHealthID Endorsement – Endorsement Request Expired",
                body: templateBody);

            await this.emailService.SendAsync(email);

            inactiveEndorsement.Status = EndorsementRequestStatus.Cancelled;
            inactiveEndorsement.StatusDate = this.clock.GetCurrentInstant();
            await this.context.SaveChangesAsync(stoppingToken);
        }

        //7 days notification alert
        endorsements = await this.context.EndorsementRequests
                                   .Where(request =>
                                       (this.clock.GetCurrentInstant() - request.StatusDate).Days == 7 &&
                                       request.Status == EndorsementRequestStatus.Created)
                                   .ToListAsync(stoppingToken);

        template = System.IO.File.ReadAllText("EmailTemplate/EndorsementAcitionRequired.html");

        foreach (var endorsementRequest in endorsements)
        {
            var templateBody = template.ToString();
            templateBody = templateBody.Replace("_OneHealthUrl", oneHealthUrl);//Replacing OneHealth Url

            var email = new Email(
                from: EmailService.PidpEmail,
                to: endorsementRequest.RecipientEmail,
                subject: $"OneHealthID Endorsement - Action Required",
                body: templateBody);

            if (this.CheckEmailStatus(endorsementRequest.RecipientEmail).Result)
            {
                await this.emailService.SendAsync(email);
            }
        }

    }
    private async Task<bool> CheckEmailStatus(string email)
    {
        Expression<Func<EmailLog, bool>> predicate = log =>
          log.SendType == "SMTP"
          && log.Created.ToDateTimeUtc().Date == this.clock.GetCurrentInstant().ToDateTimeUtc().Date
          && log.SentTo == email;

        var totalCount = await this.context.EmailLogs
            .Where(predicate)
            .CountAsync();

        return totalCount == 0;
    }
}

