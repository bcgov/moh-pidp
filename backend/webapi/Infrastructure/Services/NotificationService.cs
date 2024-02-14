namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using NodaTime;
using Pidp.Data;
using Pidp.Data.Migrations;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models;

public sealed class NotificationService : INotificationService
{
    private readonly PidpDbContext context;
    private readonly IEmailService emailService;
    private readonly IClock clock;

    public NotificationService(
        PidpDbContext context,
       IEmailService emailService,
        IClock clock
        )
    {
        this.context = context;
        this.emailService = emailService;
        this.clock = clock;
    }

    public async Task SendEndorsementInactiveNotification(CancellationToken stoppingToken)
    {
        var endorsements = await this.context.EndorsementRequests
                                    .Where(request =>
                                        (this.clock.GetCurrentInstant() - request.StatusDate).Days > 30 &&
                                        (request.Status != EndorsementRequestStatus.Cancelled || request.Status != EndorsementRequestStatus.Declined))
                                    .ToListAsync(stoppingToken);

        var template = System.IO.File.ReadAllText("EmailTemplate/EndorsementInactiveNotification.html");

        foreach (var inactiveEndorsement in endorsements)
        {
            var userName = string.Empty;
            var endorsementType = string.Empty;

            if (inactiveEndorsement.RequestingPartyId != 0)
            {
                userName = await this.context.Parties
               .Where(party => party.Id == inactiveEndorsement.RequestingPartyId)
               .Select(party => party.FullName)
               .SingleAsync(cancellationToken: stoppingToken);
                endorsementType = "Request";
            }
            else if (inactiveEndorsement.ReceivingPartyId != 0)
            {
                userName = await this.context.Parties
              .Where(party => party.Id == inactiveEndorsement.ReceivingPartyId)
              .Select(party => party.FullName)
              .SingleAsync(cancellationToken: stoppingToken);
                endorsementType = "Response";
            }

            var templateBody = template.ToString();
            templateBody = templateBody.Replace("_Name", userName);//Replacing User Name
            templateBody = templateBody.Replace("_EndorsementType", endorsementType);//Replacing Endorsement Type

            var email = new Email(
                from: EmailService.PidpEmail,
                to: inactiveEndorsement.RecipientEmail,
                subject: $"Inactivity Endorsement, please do the necessary action",
                body: templateBody);

            await this.emailService.SendAsync(email);

            inactiveEndorsement.Status = EndorsementRequestStatus.Cancelled;
            inactiveEndorsement.StatusDate = this.clock.GetCurrentInstant();
            await this.context.SaveChangesAsync(stoppingToken);
        }

    }
}

