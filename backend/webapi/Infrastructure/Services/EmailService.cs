namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models;
using MassTransit;

public class EmailService(
    IBus bus,
    IChesClient chesClient,
    ISmtpEmailClient smtpEmailClient,
    PidpConfiguration config,
    PidpDbContext context) : IEmailService
{
    public const string PidpEmail = "OneHealthID@gov.bc.ca";
    public const string PidpSupportPhone = "250-448-1262";

    private readonly IBus bus = bus;
    private readonly IChesClient chesClient = chesClient;
    private readonly ISmtpEmailClient smtpEmailClient = smtpEmailClient;
    private readonly PidpConfiguration config = config;
    private readonly PidpDbContext context = context;

    public async Task SendAsync(Email email)
    {
        if (!PidpConfiguration.IsProduction())
        {
            email.Subject = $"THE FOLLOWING EMAIL IS A TEST: {email.Subject}";
        }

        if (this.config.ChesClient.Enabled)
        {
            bus.Publish(email);
        }
        else
        {
            await this.smtpEmailClient.SendAsync(email);
        }
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

    private static class SendType
    {
        public const string Ches = "CHES";
    }
}
