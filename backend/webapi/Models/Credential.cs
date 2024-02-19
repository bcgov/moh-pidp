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

    public Guid UserId { get; set; }

    public string? IdentityProvider { get; set; }

    public string? IdpId { get; set; }

    [Projectable]
    public string? Hpdid => this.IsBcServicesCard ? this.IdpId : null;

    [Projectable]
    public bool IsBcServicesCard => this.IdentityProvider == IdentityProviders.BCServicesCard;

    /// <summary>
    /// The expected Ministry of Health Keycloak username for this user. The schema is {IdentityProviderIdentifier}@{IdentityProvider}.
    /// Most of our Credentials come to us from Keycloak and so the username is saved as-is in the column IdpId.
    /// However, we create BC Provider Credentials directly; so the User Principal Name is saved to the database without the suffix.
    /// There are also two inconsistencies with how the MOH Keycloak handles BCP usernames:
    /// 1. The username suffix is @bcp rather than @bcprovider_aad.
    /// 2. This suffix is only added in Test and Production; there is no suffix at all for BCP Users in the Dev environment.
    /// </summary>
    public string? GenerateMohKeycloakUsername()
    {
        if (this.IdentityProvider == IdentityProviders.BCProvider && !PidpConfiguration.IsDevelopment())
        {
            return this.IdpId + "@bcp";
        }

        return this.IdpId;
    }
}
