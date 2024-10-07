namespace Pidp.Infrastructure.HttpClients.Mail;

using System.Net.Mail;

using static Pidp.PidpConfiguration;

public class SmtpEmailClient(ILogger<SmtpEmailClient> logger, PidpConfiguration config) : ISmtpEmailClient
{
    private readonly ILogger<SmtpEmailClient> logger = logger;
    private readonly MailServerConfiguration config = config.MailServer;

    public async Task SendAsync(Email email)
    {
        using var mail = ConvertToMailMessage(email);
        using var smtp = new SmtpClient(this.config.Url, this.config.Port)
        {
            EnableSsl = this.config.EnableSsl
        };

        try
        {
            await smtp.SendMailAsync(mail);
        }
        catch (Exception e)
        {
            this.logger.LogSmtpClientException(e);
        }
    }

    private static MailMessage ConvertToMailMessage(Email email)
    {
        var mail = new MailMessage
        {
            From = new MailAddress(email.From),
            Subject = email.Subject,
            Body = email.Body,
            IsBodyHtml = true,
        };

        foreach (var address in email.To)
        {
            mail.To.Add(address);
        }

        foreach (var address in email.Cc)
        {
            mail.CC.Add(address);
        }

        foreach (var attachment in email.Attachments)
        {
            mail.Attachments.Add(new Attachment(new MemoryStream(attachment.Data), attachment.Filename, attachment.MediaType));
        }

        return mail;
    }
}

public static partial class SmtpClientLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Unhandled exception when sending Email via SMTP.")]
    public static partial void LogSmtpClientException(this ILogger<SmtpEmailClient> logger, Exception e);
}
