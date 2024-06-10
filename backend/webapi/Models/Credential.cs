namespace Pidp.Models;

using EntityFrameworkCore.Projectables;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Infrastructure.Auth;

[Table(nameof(Credential))]
public class Credential : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    /// <summary>
    /// The Id of the User in Keycloak.
    /// Will only be all zeros in the very short window between when a new BC Provider account + Credential is created and when the corresponding Keycloak account is created and the DB is updated.
    /// </summary>
    public Guid UserId { get; set; }

    public string? IdentityProvider { get; set; }

    public string? IdpId { get; set; }

    [Projectable]
    public string? Hpdid => this.IsBcServicesCard ? this.IdpId : null;

    [Projectable]
    public bool IsBcServicesCard => this.IdentityProvider == IdentityProviders.BCServicesCard;
}
