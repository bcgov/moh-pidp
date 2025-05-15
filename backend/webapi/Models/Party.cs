namespace Pidp.Models;

using EntityFrameworkCore.Projectables;
using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
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

    /// <summary>
    /// A unique 20 digit alphanumeric identifier for a given Party accross all of their MoH accounts/credentials
    /// </summary>
    public string? OpId { get; set; }

    public LocalDate? Birthdate { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? PreferredFirstName { get; set; }

    public string? PreferredMiddleName { get; set; }

    public string? PreferredLastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Cpn { get; set; }

    public PartyLicenceDeclaration? LicenceDeclaration { get; set; }

    public ICollection<AccessRequest> AccessRequests { get; set; } = [];

    public ICollection<Credential> Credentials { get; set; } = [];

    public ICollection<InvitedEntraAccount> InvitedEntraAccounts { get; set; } = [];

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
    /// The "primary" Credential of a Party is the a) BC Services Card Credential or b) the only non-BC Services Card Credential.
    /// </summary>
    [Projectable]
    public Guid PrimaryUserId => this.Credentials
        .OrderByDescending(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard)
        .Select(credential => credential.UserId)
        .First();

    [Projectable]
    public IEnumerable<string> Upns => this.Credentials
        .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
        .Select(credential => credential.IdpId!)
        .Union(this.InvitedEntraAccounts
            .Select(account => account.UserPrincipalName));

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

        this.DomainEvents.Add(new PlrCpnLookupFound(this.Id, this.Credentials.Select(credential => credential.UserId), this.Cpn, standingsDigest));
        this.DomainEvents.Add(new CollegeLicenceUpdated(this.Id));

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

    /// <summary>
    /// Assigns a unique OpId to this Party if it does not already have one.
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task GenerateOpId(PidpDbContext context)
    {
        if (this.OpId != null)
        {
            return;
        }

        string opId;
        var attempts = 100;

        do
        {
            if (--attempts == 0)
            {
                throw new InvalidOperationException("Maximum attempts to generate a unique OpId exceeded");
            }
            opId = Nanoid.Generate("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", 20);
        } while (await context.Parties.AnyAsync(party => party.OpId == opId)); // NOTE: case sensitive!

        this.OpId = opId;
    }
}
