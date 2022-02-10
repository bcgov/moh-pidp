namespace Pidp.Infrastructure.HttpClients.Mail;

using System;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;

public class SmtpEmailClient : ISmtpEmailClient
{
    private readonly ILogger<SmtpEmailClient> logger;
    private readonly string url;
    private readonly int port;

    public SmtpEmailClient(ILogger<SmtpEmailClient> logger, PidpConfiguration config)
    {
        this.logger = logger;
        this.url = config.MailServer.Url;
        this.port = config.MailServer.Port;
    }

    public async Task SendAsync(Email email)
    {
        var mail = ConvertToMailMessage(email);
        var smtp = new SmtpClient(this.url, this.port);

        try
        {
            await smtp.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            if (ex is InvalidOperationException
                or SmtpException
                or SmtpFailedRecipientException
                or SmtpFailedRecipientsException)
            {
                // TODO add logging for mail exception
                Console.WriteLine($"SmtpEmailClient exception: {ex}");
            }

            throw;
        }
        finally
        {
            smtp.Dispose();
            mail.Dispose();
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
