namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(CredentialLinkErrorLog))]
public class CredentialLinkErrorLog : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int ExistingCredentialId { get; set; }

    public Credential? ExistingCredential { get; set; }

    public int CredentialLinkTicketId { get; set; }

    public CredentialLinkTicket? CredentialLinkTicket { get; set; }
}
