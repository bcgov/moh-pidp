namespace Pidp.Infrastructure.HttpClients.Mail;

public class ChesEmailRequestParams
{
    public IEnumerable<ChesAttachment> Attachments { get; set; }
    public IEnumerable<string> Bcc { get; set; }
    public string BodyType { get; set; }
    public string Body { get; set; }
    public IEnumerable<string> Cc { get; set; }
    public int? DelayTS { get; set; }
    public string Encoding { get; set; }
    public string From { get; set; }
    public string Priority { get; set; }
    public string Subject { get; set; }
    public string Tag { get; set; }
    public IEnumerable<string> To { get; set; }

    public ChesEmailRequestParams()
    {
        // Defaults
        this.Bcc = Enumerable.Empty<string>();
        this.BodyType = "html";
        this.DelayTS = 0;
        this.Encoding = "utf-8";
        this.Priority = "normal";
        this.Tag = "tag";
    }

    public ChesEmailRequestParams(Email email)
        : this()
    {
        this.Attachments = email.Attachments.Select(file => new ChesAttachment()
        {
            Content = Convert.ToBase64String(file.Data),
            ContentType = file.MediaType,
            Encoding = "base64",
            Filename = file.Filename
        });

        this.Body = email.Body;
        this.Cc = email.Cc;
        this.From = email.From;
        this.Subject = email.Subject;
        this.To = email.To;
    }
}

public class ChesAttachment
{
    public string Content { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string Encoding { get; set; } = string.Empty;
    public string Filename { get; set; } = string.Empty;
}

public class EmailSuccessResponse
{
    public IEnumerable<Message> Messages { get; set; } = Enumerable.Empty<Message>();
    public Guid TxId { get; set; }
}

public class Message
{
    public Guid MsgId { get; set; }
    public string Tag { get; set; } = string.Empty;
    public IEnumerable<string> To { get; set; } = Enumerable.Empty<string>();
}

public class StatusResponse
{
    public long CreatedTS { get; set; }
    public long DelayTS { get; set; }
    public Guid MsgId { get; set; }
    public string Status { get; set; } = string.Empty;
    public IEnumerable<StatusHistoryObject> StatusHistory { get; set; } = Enumerable.Empty<StatusHistoryObject>();
    public string Tag { get; set; } = string.Empty;
    public Guid TxId { get; set; }
    public long UpdatedTS { get; set; }
}

public class StatusHistoryObject
{
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Timestamp { get; set; }
}

public static class ChesStatus
{
    public const string Accepted = "accepted";
    public const string Cancelled = "cancelled";
    public const string Completed = "completed";
    public const string Failed = "failed";
    public const string Pending = "pending";
}
