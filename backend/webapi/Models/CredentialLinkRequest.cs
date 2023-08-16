namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table(nameof(CredentialLinkRequest))]
public class CredentialLinkRequest : BaseAuditable
{
    public static readonly Duration Expiry = Duration.FromHours(1);

    [Key]
    public Guid Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public Guid UserId { get; set; }

    public string? IdentityProvider { get; set; }

    public string? IdpId { get; set; }
}
