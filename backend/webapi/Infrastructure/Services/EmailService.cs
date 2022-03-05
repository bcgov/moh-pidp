namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Linq.Expressions;

using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models;

public class EmailService : IEmailService
{
    private const string PidpEmail = "provideridentityportal@gov.bc.ca";
    private readonly IChesClient chesClient;
    private readonly IClock clock;
    private readonly ILogger logger;
    private readonly ISmtpEmailClient smtpEmailClient;
    private readonly PidpConfiguration config;
    private readonly PidpDbContext context;

    public EmailService(
        IChesClient chesClient,
        IClock clock,
        ILogger<EmailService> logger,
        ISmtpEmailClient smtpEmailClient,
        PidpConfiguration config,
        PidpDbContext context)
    {
        this.chesClient = chesClient;
        this.clock = clock;
        this.logger = logger;
        this.smtpEmailClient = smtpEmailClient;
        this.config = config;
        this.context = context;
    }

    public async Task SendSaEformsAccessRequestConfirmationAsync(int partyId)
    {
        var party = await this.context.Parties
            .Where(party => party.Id == partyId)
            .Select(party => new
            {
                party.FirstName,
                party.Email
            })
            .SingleOrDefaultAsync();

        if (party?.Email == null)
        {
            this.logger.LogNullPartyEmail(partyId);
            return;
        }

        var url = "https://www.eforms.phsahealth.ca/appdash";
        var link = $"<a href=\"{url}\" target=\"_blank\" rel=\"noopener noreferrer\">link</a>";
        var email = new Email(
            from: PidpEmail,
            to: party.Email,
            subject: "SA eForms Enrolment Confirmation",
            body: $"Hi {party.FirstName},<br><br>You will need to visit this {link} each time you want to submit an SA eForm. It may be helpful to bookmark this {link} for future use."
        );
        await this.Send(email);
    }

    private async Task Send(Email email)
    {
        if (!PidpConfiguration.IsProduction())
        {
            email.Subject = $"THE FOLLOWING EMAIL IS A TEST: {email.Subject}";
        }

        if (this.config.ChesClient.Enabled && await this.chesClient.HealthCheckAsync())
        {
            var msgId = await this.chesClient.SendAsync(email);
            await this.CreateEmailLog(email, SendType.Ches, msgId);

            if (msgId != null)
            {
                return;
            }
        }

        // Fall back to SMTP client
        await this.CreateEmailLog(email, SendType.Smtp);
        await this.smtpEmailClient.SendAsync(email);
    }

    public async Task<int> UpdateEmailLogStatuses(int limit)
    {
        Expression<Func<EmailLog, bool>> predicate = log =>
            log.SendType == SendType.Ches
            && log.MsgId != null
            && log.LatestStatus != ChesStatus.Completed;

        var totalCount = await this.context.EmailLogs
            .Where(predicate)
            .CountAsync();

        var emailLogs = await this.context.EmailLogs
            .Where(predicate)
            .OrderBy(e => e.UpdateCount)
                .ThenBy(e => e.Modified)
            .Take(limit)
            .ToListAsync();

        foreach (var emailLog in emailLogs)
        {
            var status = await this.chesClient.GetStatusAsync(emailLog.MsgId!.Value);
            if (status != null && emailLog.LatestStatus != status)
            {
                emailLog.LatestStatus = status;
            }
            emailLog.UpdateCount++;
        }
        await this.context.SaveChangesAsync();

        return totalCount;
    }

    private async Task CreateEmailLog(Email email, string sendType, Guid? msgId = null)
    {
        this.context.EmailLogs.Add(new EmailLog(email, sendType, msgId, this.clock.GetCurrentInstant()));
        await this.context.SaveChangesAsync();
    }

    private static class SendType
    {
        public const string Ches = "CHES";
        public const string Smtp = "SMTP";
    }
}

public static partial class EmailServiceLogingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "No email address found for Party with Id {partyId}.")]
    public static partial void LogNullPartyEmail(this ILogger logger, int partyId);
}
