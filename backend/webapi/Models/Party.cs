namespace Pidp.Models;

using EntityFrameworkCore.Projectables;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.DomainEvents;

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

    public string? OpId { get; set; }

    /// <summary>
    /// The First Name + Last Name of the Party.
    /// </summary>
    [Projectable]
    public string FullName => $"{this.FirstName} {this.LastName}";

    /// <summary>
    /// First name to display.
    /// The preferred first name if provided otherwise the first name.
    /// </summary>
    [Projectable]
    public string DisplayFirstName => this.PreferredFirstName ?? this.FirstName;

    /// <summary>
    /// Last name to display.
    /// The preferred last name if provided otherwise the last name.
    /// </summary>
    [Projectable]
    public string DisplayLastName => this.PreferredLastName ?? this.LastName;

    /// <summary>
    /// The full name to display.
    /// The display first name + display last name.
    /// </summary>
    [Projectable]
    public string DisplayFullName => $"{this.DisplayFirstName} {this.DisplayLastName}";

    /// <summary>
    /// The "primary" Credential of a Party is the a) only, or b) BC Services Card Credential.
    /// As of now, the only Parties that have two Credentials are first BC Services Card and then later recieve a BC Provider Credential.
    /// </summary>
    [Projectable]
    public Guid PrimaryUserId => this.Credentials
        .Single(credential => credential.IdentityProvider != IdentityProviders.BCProvider)
        .UserId;

    /// <summary>
    /// Uses the Party's Licence Declaration to search PLR for records.
    /// Must have Credentials and PartyLicenceDeclaration loaded.
    /// </summary>
    public async Task HandleLicenceSearch(IPlrClient client, PidpDbContext context)
    {
        // TODO: Check children are loaded?
        if (this.Birthdate == null
            || this.LicenceDeclaration?.CollegeCode == null
            || string.IsNullOrWhiteSpace(this.LicenceDeclaration?.LicenceNumber))
        {
            return;
        }

        this.Cpn = await client.FindCpnAsync(this.LicenceDeclaration.CollegeCode.Value, this.LicenceDeclaration.LicenceNumber, this.Birthdate.Value);
        if (string.IsNullOrWhiteSpace(this.Cpn))
        {
            return;
        }

        var standingsDigest = await client.GetStandingsDigestAsync(this.Cpn);

        this.DomainEvents.Add(new PlrCpnLookupFound(this.Id, this.PrimaryUserId, this.Cpn, standingsDigest));

        if (standingsDigest.HasGoodStanding)
        {
            var endorsingPartyIds = await context.ActiveEndorsementRelationships(this.Id)
                .Select(relationship => relationship.PartyId)
                .Distinct()
                .ToListAsync();

            foreach (var partyId in endorsingPartyIds)
            {
                this.DomainEvents.Add(new EndorsementStandingUpdated(partyId));
            }
        }
    }
}
