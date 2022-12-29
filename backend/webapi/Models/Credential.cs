namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum CredentialType
{
    Unknown = 0,
    Bcsc,
    BcProvider,
    Idir,
    Phsa
}

[Table(nameof(Credential))]
public class Credential : BaseAuditable, IOwnedResource
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string IdpId { get; set; } = string.Empty;

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public CredentialType CredentialType { get; set; }

    // Computed properties
    public string? Hpdid => this.CredentialType == CredentialType.Bcsc ? this.IdpId : null;
}
