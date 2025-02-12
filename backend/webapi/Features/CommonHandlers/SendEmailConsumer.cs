namespace Pidp.Features.CommonHandlers;

using MassTransit;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models;

public class SendEmailConsumer(
    IClock clock,
    IChesClient chesClient,
    ILogger<SendEmailConsumer> logger,
    PidpDbContext context) : IConsumer<Email>
{
    private readonly IClock clock = clock;
    private readonly IChesClient chesClient = chesClient;
    private readonly PidpDbContext context = context;
    private readonly ILogger<SendEmailConsumer> logger = logger;

    public async Task Consume(ConsumeContext<Email> context)
    {
        Console.WriteLine("Received email message");
        var message = context.Message;

        var email = new Email(
            from: message.From,
            to: message.To,
            cc: message.Cc ?? [],
            subject: message.Subject,
            body: message.Body,
            attachments: message.Attachments ?? []
        );


        if (!await this.chesClient.HealthCheckAsync())
        {
            this.logger.LogChesClientHealthCheckFailure();
            throw new InvalidOperationException("Error communicating with CHES API");
        }

        // Call the CHES API to send the email
        var msgId = await this.chesClient.SendAsync(email);
        if (msgId != null)
        {
            await this.CreateEmailLog(email, "CHES", msgId);
            Console.WriteLine("Email sent Successfully");
        }

        this.logger.LogSendEmailFailure();
        throw new InvalidOperationException("Error sending email");
    }

    private async Task CreateEmailLog(Email email, string sendType, Guid? msgId = null)
    {
        this.context.EmailLogs.Add(new EmailLog(email, sendType, msgId, this.clock.GetCurrentInstant()));
        await this.context.SaveChangesAsync();
    }
}

internal static partial class SendEmailConsumerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error communicating with CHES API.")]
    public static partial void LogChesClientHealthCheckFailure(this ILogger<SendEmailConsumer> logger);

    [LoggerMessage(2, LogLevel.Error, "Error sending email")]
    public static partial void LogSendEmailFailure(this ILogger<SendEmailConsumer> logger);
}
