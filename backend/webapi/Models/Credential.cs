namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum CredentialType
{
    Bcsc = 1,
    BcProvider,
    Idir,
    HealthAuthority

}

[Table(nameof(Credential))]
public abstract class Credential : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string IdpId { get; set; } = string.Empty;

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public CredentialType CredentialType { get; set; }
}
