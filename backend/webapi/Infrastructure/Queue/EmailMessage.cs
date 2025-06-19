namespace Pidp.Infrastructure.HttpClients.Mail;

using MassTransit;

public class EmailMessage
{
    public string From { get; set; }
    public IEnumerable<string> To { get; set; }
    public IEnumerable<string> Cc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public MessageData<byte[]>? Document { get; set; }
    public string? Filename { get; set; }
    public string? MediaType { get; set; }

    public EmailMessage()
    {
        this.From = string.Empty;
        this.To = [];
        this.Cc = [];
        this.Subject = string.Empty;
        this.Body = string.Empty;
        this.Document = null;
        this.Filename = string.Empty;
        this.MediaType = string.Empty;
    }

}
