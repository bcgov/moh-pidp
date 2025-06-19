namespace Pidp.Infrastructure.HttpClients.Mail;

using System.Net.Mail;

public class Email
{
    public string From { get; set; }
    public IEnumerable<string> To { get; set; }
    public IEnumerable<string> Cc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public IEnumerable<File> Attachments { get; set; }

    public Email()
    {
        this.From = string.Empty;
        this.To = [];
        this.Cc = [];
        this.Subject = string.Empty;
        this.Body = string.Empty;
        this.Attachments = [];
    }

    public Email(string from, string to, string subject, string body)
        : this(from, [to], [], subject, body, [])
    { }

    public Email(string from, string to, string cc, string subject, string body)
        : this(from, [to], [cc], subject, body, [])
    { }

    public Email(string from, IEnumerable<string> to, string subject, string body)
        : this(from, to, [], subject, body, [])
    { }

    public Email(string from, IEnumerable<string> to, string cc, string subject, string body)
        : this(from, to, [cc], subject, body, [])
    { }

    public Email(string from, IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, IEnumerable<File> attachments)
    {
        ValidateEmails(from, to, cc);

        this.From = from;
        this.To = to;
        this.Cc = cc;
        this.Subject = subject;
        this.Body = body;
        this.Attachments = attachments;
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static void ValidateEmails(string from, IEnumerable<string> to, IEnumerable<string> cc)
    {
        if (!IsValidEmail(from))
        {
            throw new ArgumentException($"\"From\" email {from} is invalid");
        }

        foreach (var email in to)
        {
            if (!IsValidEmail(email))
            {
                throw new ArgumentException($"\"To\" email {email} is invalid");
            }
        }

        foreach (var email in cc)
        {
            if (!IsValidEmail(email))
            {
                throw new ArgumentException($"\"CC\" email {email} is invalid");
            }
        }
    }
}
