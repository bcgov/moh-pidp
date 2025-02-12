namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models;
using MassTransit;

public class EmailService(
    IChesClient chesClient,
    ISendEndpointProvider sendEndpointProvider,
    PidpConfiguration config,
    PidpDbContext context) : IEmailService
{
    public const string PidpEmail = "OneHealthID@gov.bc.ca";
    public const string PidpSupportPhone = "250-448-1262";

    private readonly IChesClient chesClient = chesClient;
    private readonly PidpConfiguration config = config;
    private readonly PidpDbContext context = context;
    private readonly ISendEndpointProvider sendEndpointProvider = sendEndpointProvider;

    public async Task SendAsync(Email email)
    {
        if (!PidpConfiguration.IsProduction())
        {
            email.Subject = $"THE FOLLOWING EMAIL IS A TEST: {email.Subject}";
        }

        if (this.config.ChesClient.Enabled)
        {
            var sendEndpoint = await this.sendEndpointProvider.GetSendEndpoint(new Uri("queue:send-email-queue"));
            await sendEndpoint.Send(email);
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
