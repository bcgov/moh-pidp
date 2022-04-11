namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Infrastructure.HttpClients.Mail;

[Table(nameof(EmailLog))]
public class EmailLog : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public string SendType { get; set; } = string.Empty;

    public Guid? MsgId { get; set; }

    public string SentTo { get; set; } = string.Empty;

    public string Cc { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public Instant? DateSent { get; set; }

    public string? LatestStatus { get; set; }

    public string? StatusMessage { get; set; }

    public int UpdateCount { get; set; }

    public EmailLog() { }

    public EmailLog(Email email, string sendType, Guid? msgId, Instant dateSent)
    {
        this.Body = email.Body;
        this.Cc = string.Join(",", email.Cc);
        this.DateSent = dateSent;
        this.MsgId = msgId;
        this.SendType = sendType;
        this.SentTo = string.Join(",", email.To);
        this.Subject = email.Subject;
    }
}
