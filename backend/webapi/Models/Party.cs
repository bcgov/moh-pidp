namespace Pidp.Models;

using EntityFrameworkCore.Projectables;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Infrastructure.Auth;

[Table(nameof(Party))]
public class Party : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public LocalDate? Birthdate { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? PreferredFirstName { get; set; }

    public string? PreferredMiddleName { get; set; }

    public string? PreferredLastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public PartyAccessAdministrator? AccessAdministrator { get; set; }

    public string? Cpn { get; set; }

    public PartyLicenceDeclaration? LicenceDeclaration { get; set; }

    public string? JobTitle { get; set; }

    public Facility? Facility { get; set; }

    public PartyOrgainizationDetail? OrgainizationDetail { get; set; }

    public ICollection<AccessRequest> AccessRequests { get; set; } = new List<AccessRequest>();

    public ICollection<Credential> Credentials { get; set; } = new List<Credential>();

    /// <summary>
    /// The First Name + Last Name of the Party.
    /// </summary>
    [Projectable]
    public string FullName => $"{this.FirstName} {this.LastName}";

    /// <summary>
    /// The "primary" Credential of a Party is the a) only, or b) BC Services Card Credential.
    /// As of now, the only Parties that have two Credentials are first BC Services Card and then later recieve a BC Provider Credential.
    /// </summary>
    [Projectable]
    public Guid PrimaryUserId => this.Credentials
        .Single(credential => credential.IdentityProvider != IdentityProviders.BCProvider)
        .UserId;
}
