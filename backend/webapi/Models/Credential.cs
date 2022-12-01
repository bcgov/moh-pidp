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

    public Party? Party { get; set; }

    public Guid UserId { get; set; }

    public string? IdentityProvider { get; set; }

    public string? IdpId { get; set; }

    [Projectable]
    public string? Hpdid => this.IsBcServicesCard ? this.IdpId : null;

    [Projectable]
    public bool IsBcServicesCard => this.IdentityProvider == IdentityProviders.BCServicesCard;
}
