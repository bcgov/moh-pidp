namespace EndorsementReminder;

using Flurl;
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
            .Include(request => request.RequestingParty)
            .Where(request => inProgressStatuses.Contains(request.Status)
                && request.StatusDate < now - Duration.FromDays(30))
            .ToListAsync();

        foreach (var request in oldRequests)
        {
            var email = request.RequestingParty!.Email;

            if (!string.IsNullOrEmpty(email))
            {
                var expiresAt = request.StatusDate.Plus(Duration.FromDays(30));
                this.logger.LogSendingExpiryEmail(request.Id, request.StatusDate, expiresAt);
                await this.SendExpireOldEndorsementRequestsEmailAsync(email);
                request.Status = EndorsementRequestStatus.Expired;
                request.StatusDate = now;
            }
        }

        await this.context.SaveChangesAsync();
        this.logger.LogExpiredRequestCount(oldRequests.Count);
    }

    public async Task SendReminderEmailsAsync()
    {
        var inProgressStatuses = new[] { EndorsementRequestStatus.Created, EndorsementRequestStatus.Received, EndorsementRequestStatus.Approved };
        var now = this.clock.GetCurrentInstant();
        var emailDtos = await this.context.EndorsementRequests
            .Where(request => inProgressStatuses.Contains(request.Status)
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
                Id = request.Id,
                StatusDate = request.StatusDate,
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
            var expiresAt = recipient.StatusDate.Plus(Duration.FromDays(30));
            this.logger.LogSendingReminderEmail(recipient.Id, recipient.StatusDate, expiresAt);
            await this.SendEmailAsync(recipient.Email, recipient.Token);
        }

        this.logger.LogSentEmailCount(uniqueRecipients.Count());
    }

    private async Task SendEmailAsync(string partyEmail, Guid? token)
    {
        var email = new Email(
            from: EmailService.PidpEmail,
            to: partyEmail,
            subject: "OneHealthID Endorsement - Action Required",
            body: this.GetBodyString(token, true)
        );
        await this.emailService.SendAsync(email);
    }

    private async Task SendExpireOldEndorsementRequestsEmailAsync(string partyEmail)
    {
        var email = new Email(
            from: EmailService.PidpEmail,
            to: partyEmail,
            subject: "OneHealthID Endorsement – Endorsement Request Expired",
            body: this.GetBodyString()
        );
        await this.emailService.SendAsync(email);
    }

    private string GetBodyString(Guid? token = null, bool is7DaysNotify = false)
    {
        var url = this.applicationUrl.SetQueryParam("endorsement-token", token);
        var link = $"<a href=\"{url}\" target=\"_blank\" rel=\"noopener noreferrer\">OneHealthID Service</a>";
        var licensedlink = $"<a href=\"https://www.doctorsofbc.ca/sites/default/files/endorsement_licensed_initiated_4.pdf\" target=\"_blank\" rel=\"noopener noreferrer\">Licensed Initiated</a>";
        var unLicensedlink = $"<a href=\"https://www.doctorsofbc.ca/sites/default/files/endorsement_unlicensed_initiated_4.pdf\" target=\"_blank\" rel=\"noopener noreferrer\">Unlicensed Initiated</a>";
        var pidpSupportEmail = $"<a href=\"mailto:{EmailService.PidpEmail}\">{EmailService.PidpEmail}</a>";
        var pidpSupportPhone = $"<a href=\"tel:{EmailService.PidpSupportPhone}\">{EmailService.PidpSupportPhone}</a>";

        var bodyString = $@"Hello,
<br>
<br>This email is to inform you that your endorsement request has now expired due to inactivity of 30 days or more.
<br>
<br>If you would like to restart the process and set up an endorsement, log into the OneHealthID Service with your BC Services Card app: {link}
<br>
<br>For additional support and information on the endorsement process,
please refer to these infographics hosted on the Doctors of BC website here: {licensedlink} or {unLicensedlink}.
Or contact the OneHealthID Service desk:
<br>
<br>&emsp;• By email at {pidpSupportEmail}
<br>
<br>&emsp;• By phone at {pidpSupportPhone}
<br>
<br>Thank you.";

        if (is7DaysNotify)
        {
            bodyString = $@"Hello,
<br>
<br>You recently started an endorsement request with another individual in OneHealthID.
Users have 30 days to complete the endorsement process before it expires.
Your request has been inactive for 7 days. To complete the pending endorsements,
log into the OneHealthID Service with your BC Services Card app: {link}
<br>
<br>After logging in, please complete these steps, if applicable:
<br>&emsp;1. Mandatory first-time login steps (if this is your first time logging in to OneHealthID).
<br>&emsp;2. Review the pending endorsements in the “Endorsements” tile under “Organization Info”.
<br>&emsp;3. Each pending endorsement will be listed under “Incoming Requests.” To confirm the endorsement, click “Approve” next to the request.
<br>
<br>For additional support and information on the endorsement process,
please refer to these infographics hosted on the Doctors of BC website here: {licensedlink} or {unLicensedlink}.
Or contact the OneHealthID Service desk:
<br>
<br>&emsp;• By email at {pidpSupportEmail}
<br>
<br>&emsp;• By phone at {pidpSupportPhone}
<br>
<br>Thank you.";
        }
        return bodyString;
    }
}

public static partial class EndorsementReminderServiceLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Sent {EmailCount} Endorsement Reminder Emails.")]
    public static partial void LogSentEmailCount(this ILogger logger, int emailCount);

    [LoggerMessage(2, LogLevel.Information, "Expired {ExpiryCount} Endorsement Requests.")]
    public static partial void LogExpiredRequestCount(this ILogger logger, int expiryCount);

    [LoggerMessage(3, LogLevel.Information, "Sending endorsement reminder email for Endorsement Request #{RequestId}. Last actioned: {LastActioned}, Expires: {ExpiresAt}.")]
    public static partial void LogSendingReminderEmail(this ILogger logger, int requestId, Instant lastActioned, Instant expiresAt);

    [LoggerMessage(4, LogLevel.Information, "Sending endorsement expiry email for Endorsement Request #{RequestId}. Last actioned: {LastActioned}, Expired: {ExpiresAt}.")]
    public static partial void LogSendingExpiryEmail(this ILogger logger, int requestId, Instant lastActioned, Instant expiresAt);
}
