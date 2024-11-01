namespace Pidp.Infrastructure.HttpClients.Mail;

public class ChesEmailRequestParams
{
    public IEnumerable<ChesAttachment> Attachments { get; set; } = [];
    public IEnumerable<string> Bcc { get; set; } = [];
    public string BodyType { get; set; } = "html";
    public string Body { get; set; } = string.Empty;
    public IEnumerable<string> Cc { get; set; } = [];
    public int? DelayTS { get; set; } = 0;
    public string Encoding { get; set; } = "utf-8";
    public string From { get; set; }
    public string Priority { get; set; } = "normal";
    public string Subject { get; set; } = string.Empty;
    public string Tag { get; set; } = "tag";
    public IEnumerable<string> To { get; set; }

    public ChesEmailRequestParams(Email email)
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
    public IEnumerable<Message> Messages { get; set; } = [];
    public Guid TxId { get; set; }
}

public class Message
{
    public Guid MsgId { get; set; }
    public string Tag { get; set; } = string.Empty;
    public IEnumerable<string> To { get; set; } = [];
}

public class StatusResponse
{
    public long CreatedTS { get; set; }
    public long DelayTS { get; set; }
    public Guid MsgId { get; set; }
    public string Status { get; set; } = string.Empty;
    public IEnumerable<StatusHistoryObject> StatusHistory { get; set; } = [];
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
