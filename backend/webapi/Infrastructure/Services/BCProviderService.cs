namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Models;
using Pidp.Models.Lookups;

public class BCProviderService(PidpDbContext context, IBCProviderClient bcProviderClient, IClock clock) : IBCProviderService
{
    private readonly PidpDbContext context = context;
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly IClock clock = clock;

    public async Task UpdatePharmStaffAttributes(int partyId, CancellationToken cancellationToken)
    {
        var upn = await this.context.Credentials
            .Where(c => c.PartyId == partyId && c.IdentityProvider == IdentityProviders.BCProvider)
            .Select(c => c.IdpId)
            .FirstOrDefaultAsync(cancellationToken);

        if (string.IsNullOrEmpty(upn))
        {
            return;
        }

        var now = this.clock.GetCurrentInstant().ToDateTimeUtc();
        var roles = await this.context.PharmacyPartyRoles
            .Include(r => r.Pharmacy)
            .Where(r => r.PartyId == partyId
                     && (r.EffectiveEndDate == null || r.EffectiveEndDate > now))
            .ToListAsync(cancellationToken);

        if (roles.Count == 0)
        {
            return;
        }

        string jobTitle = "";
        if (roles.Any(r => r.Role == PharmacyRole.Admin))
        {
            jobTitle = "admin";
        }
        else if (roles.Any(r => r.Role == PharmacyRole.Clinician))
        {
            jobTitle = "clinician";
        }
        else if (roles.Any(r => r.Role == PharmacyRole.Clerk))
        {
            jobTitle = "clerk";
        }

        var pharmacyNames = roles.Select(r => r.Pharmacy.Name).Distinct().ToList();
        string department = string.Join(", ", pharmacyNames);

        var licenceNumber = await this.context.Parties
            .Where(p => p.Id == partyId)
            .Select(p => p.LicenceDeclaration != null ? p.LicenceDeclaration.LicenceNumber : "")
            .FirstOrDefaultAsync(cancellationToken) ?? "";

        var userUpdate = new User
        {
            JobTitle = jobTitle,
            Department = department,
            OfficeLocation = licenceNumber,
            UsageLocation = "CA"
        };

        var success = await this.bcProviderClient.UpdateUser(upn, userUpdate);
        if (success)
        {
            var details = $"JobTitle: {jobTitle}, Department: {department}, OfficeLocation: {licenceNumber}";
            this.context.BusinessEvents.Add(BCProviderAttributesUpdated.Create(partyId, details, this.clock.GetCurrentInstant()));
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
