namespace EndorsementReminder;

using Flurl;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NodaTime;

using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;

public class EndorsementMaintenanceService : IEndorsementMaintenanceService
{
    private readonly IClock clock;
    private readonly IEmailService emailService;
    private readonly ILogger logger;
    private readonly PidpDbContext context;
    private readonly string applicationUrl;

    public EndorsementMaintenanceService(
        IClock clock,
        IEmailService emailService,
        ILogger<EndorsementMaintenanceService> logger,
        PidpConfiguration config,
        PidpDbContext context)
    {
        this.clock = clock;
        this.emailService = emailService;
        this.logger = logger;
        this.context = context;
        this.applicationUrl = config.ApplicationUrl;
    }

    public async Task ExpireOldEndorsementRequestsAsync()
    {
        var inProgressStatuses = new[] { EndorsementRequestStatus.Created, EndorsementRequestStatus.Received, EndorsementRequestStatus.Approved };
        var now = this.clock.GetCurrentInstant();

        var oldRequests = await this.context.EndorsementRequests
            .Where(request => inProgressStatuses.Contains(request.Status)
                && request.StatusDate < now - Duration.FromDays(30))
            .ToListAsync();

        foreach (var request in oldRequests)
        {
            request.Status = EndorsementRequestStatus.Expired;
            request.StatusDate = now;
        }

        await this.context.SaveChangesAsync();
        this.logger.LogExpiredRequestCount(oldRequests.Count);
    }

    public async Task SendReminderEmailsAsync()
    {
        var reminderStatuses = new[] { EndorsementRequestStatus.Created, EndorsementRequestStatus.Received, EndorsementRequestStatus.Approved };
        var now = this.clock.GetCurrentInstant();
        var emailDtos = await this.context.EndorsementRequests
            .Where(request => reminderStatuses.Contains(request.Status)
                && request.StatusDate < now - Duration.FromDays(7)
                && request.StatusDate > now - Duration.FromDays(8))
            .Where(request => !this.context.EndorsementRequests
                .Any(duplicate => duplicate.RequestingPartyId == request.RequestingPartyId
                    && duplicate.RecipientEmail == request.RecipientEmail
                    && duplicate.Status == EndorsementRequestStatus.Completed
                    && this.context.Endorsements
                        .Any(endorsement => endorsement.Active
                            && endorsement.EndorsementRelationships.Any(relation => relation.PartyId == duplicate.RequestingPartyId)
                            && endorsement.EndorsementRelationships.Any(relation => relation.PartyId == duplicate.ReceivingPartyId)))) // We should not send an email if the Parties are currently in an Active Endorsement.
            .OrderByDescending(request => request.Status)
            .Select(request => new
            {
                Token = request.Status == EndorsementRequestStatus.Created
                    ? (Guid?)request.Token
                    : null,
                Email = request.Status == EndorsementRequestStatus.Approved
                    ? request.RequestingParty!.Email!
                    : request.RecipientEmail,
            })
            .ToListAsync();

        var uniqueRecipients = emailDtos.DistinctBy(dto => dto.Email);

        foreach (var recipient in uniqueRecipients)
        {
            await this.SendEmailAsync(recipient.Email, recipient.Token);
        }

        this.logger.LogSentEmailCount(uniqueRecipients.Count());
    }

    private async Task SendEmailAsync(string partyEmail, Guid? token)
    {
        var url = this.applicationUrl.SetQueryParam("endorsement-token", token);
        var link = $"<a href=\"{url}\" target=\"_blank\" rel=\"noopener noreferrer\">{this.applicationUrl}</a>";
        var pidpSupportEmail = $"<a href=\"mailto:{EmailService.PidpEmail}\">{EmailService.PidpEmail}</a>";
        var pidpSupportPhone = $"<a href=\"tel:{EmailService.PidpSupportPhone}\">{EmailService.PidpSupportPhone}</a>";

        var email = new Email(
            from: EmailService.PidpEmail,
            to: partyEmail,
            subject: "OneHealthID Endorsement - Action Required",
            body: $@"Hello,
<br>
<br>Recently an endorsement for the OneHealthID was started either by you or another member of
your clinic. This process has not been completed. Completing the endorsement process allows
users who are not physicians or nurse practitioners to access common healthcare services, such
as the Provincial Attachment System (PAS).
<br>
<br>To complete the pending endorsement(s), log into the OneHealthID Service with your BC
Services Card app: {link}
<br>
<br>After logging in, please:
<br>&emsp;1. Complete the mandatory first time login steps (if this is your first time logging in to OneHealthID).
<br>&emsp;2. Review the pending endorsements in the “Endorsements” tile under “Organization Info”.
<br>&emsp;3. Each pending endorsement will be listed under “Incoming Requests.” To confirm the endorsement, click “Approve” next to the request.
<br>
<br>For additional support, contact the OneHealthID Service desk:
<br>
<br>&emsp;• By email at {pidpSupportEmail}
<br>
<br>&emsp;• By phone at {pidpSupportPhone}
<br>
<br>Thank you."
        );
        await this.emailService.SendAsync(email);
    }
}

public static partial class EndorsementReminderServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Sent {emailCount} Endorsement Reminder Emails.")]
    public static partial void LogSentEmailCount(this ILogger logger, int emailCount);

    [LoggerMessage(2, LogLevel.Information, "Expired {expiryCount} Endorsement Requests.")]
    public static partial void LogExpiredRequestCount(this ILogger logger, int expiryCount);
}
