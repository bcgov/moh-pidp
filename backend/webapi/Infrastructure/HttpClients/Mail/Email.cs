namespace Pidp.Infrastructure.HttpClients.Mail;

using System.Net.Mail;
using Pidp.Models;

public class Email
{
    public string From { get; set; }
    public IEnumerable<string> To { get; set; }
    public IEnumerable<string> Cc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public IEnumerable<Pdf> Attachments { get; set; }

    public Email(string from, string to, string subject, string body)
        : this(from, new[] { to }, Enumerable.Empty<string>(), subject, body, Enumerable.Empty<Pdf>())
    { }

    public Email(string from, string to, string cc, string subject, string body)
        : this(from, new[] { to }, new[] { cc }, subject, body, Enumerable.Empty<Pdf>())
    { }

    public Email(string from, IEnumerable<string> to, string subject, string body)
        : this(from, to, Enumerable.Empty<string>(), subject, body, Enumerable.Empty<Pdf>())
    { }

    public Email(string from, IEnumerable<string> to, string cc, string subject, string body)
        : this(from, to, new[] { cc }, subject, body, Enumerable.Empty<Pdf>())
    { }

    public Email(string from, IEnumerable<string> to, IEnumerable<string> cc, string subject, string body, IEnumerable<Pdf> attachments)
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
