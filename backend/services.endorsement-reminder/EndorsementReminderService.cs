namespace EndorsementReminder;

using Flurl;
using Microsoft.EntityFrameworkCore;
using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;

public class EndorsementReminderService : IEndorsementReminderService
{
    private readonly IEmailService emailService;
    private readonly PidpDbContext context;
    private readonly string applicationUrl;

    public EndorsementReminderService(
        IEmailService emailService,
        PidpConfiguration config,
        PidpDbContext context)
    {
        this.emailService = emailService;
        this.context = context;
        this.applicationUrl = config.ApplicationUrl;
    }

    public async Task DoWorkAsync()
    {
        var reminderStatuses = new[] { EndorsementRequestStatus.Created, EndorsementRequestStatus.Received, EndorsementRequestStatus.Approved };
        var emailDtos = await this.context.EndorsementRequests
            .Where(request => reminderStatuses.Contains(request.Status))
            .Select(request => new
            {
                UnRecieved = request.Status == EndorsementRequestStatus.Created,
                RequestingEmail = request.RequestingParty!.Email,
                ReceivingEmail = request.ReceivingParty!.Email
            })
            .ToListAsync();
    }

    private async Task SendEmailAsync(string partyEmail, Guid? token = null)
    {
        var url = this.applicationUrl;
        if (token.HasValue)
        {
            url.SetQueryParam("endorsement-token", token);
        }
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
