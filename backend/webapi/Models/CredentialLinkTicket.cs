namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table(nameof(CredentialLinkTicket))]
public class CredentialLinkTicket : BaseAuditable
{
    public static readonly Duration TicketLifetime = Duration.FromHours(1);

    [Key]
    public int Id { get; set; }

    public Guid Token { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public bool Claimed { get; set; }

    public Instant ExpiresAt { get; set; }

    public string LinkToIdentityProvider { get; set; } = string.Empty;
}
