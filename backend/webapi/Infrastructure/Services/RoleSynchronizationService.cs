namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models;
using Pidp.Models.Lookups;

public class RoleSynchronizationService(PidpDbContext context, IBCProviderClient bcProviderClient, IClock clock, IKeycloakAdministrationClient keycloakClient) : IRoleSynchronizationService
{
    private readonly PidpDbContext context = context;
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly IClock clock = clock;
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;

    public async Task UpdatePharmStaffAttributes(int partyId, CancellationToken cancellationToken)
    {
        var partyDetails = await this.context.Parties
            .Where(p => p.Id == partyId)
            .Select(p => new
            {
                PrimaryUserId = p.PrimaryUserId,
                LicenceNumber = p.LicenceDeclaration != null ? p.LicenceDeclaration.LicenceNumber : "",
                Upn = p.Credentials.Where(c => c.IdentityProvider == IdentityProviders.BCProvider).Select(c => c.IdpId).FirstOrDefault()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (partyDetails == null)
        {
            return;
        }

        var now = this.clock.GetCurrentInstant().ToDateTimeUtc();
        var roles = await this.context.PharmacyPartyRoles
            .Include(r => r.Pharmacy)
            .Where(r => r.PartyId == partyId
                     && (r.EffectiveEndDate == null || r.EffectiveEndDate > now))
            .ToListAsync(cancellationToken);

        string jobTitle = "";
        string department = "";
        var licenceNumber = partyDetails.LicenceNumber ?? "";
        MohKeycloakEnrolment? keycloakEnrolmentToAssign = null;

        if (roles.Count > 0)
        {
            if (roles.Any(r => r.Role == PharmacyRole.Admin))
            {
                jobTitle = "admin";
                keycloakEnrolmentToAssign = MohKeycloakEnrolment.ImmsBcPhaAdmin;
            }
            else if (roles.Any(r => r.Role == PharmacyRole.Clinician))
            {
                jobTitle = "clinician";
                keycloakEnrolmentToAssign = MohKeycloakEnrolment.ImmsBcPhaClinician;
            }
            else if (roles.Any(r => r.Role == PharmacyRole.Clerk))
            {
                jobTitle = "clerk";
                keycloakEnrolmentToAssign = MohKeycloakEnrolment.ImmsBcPhaClerk;
            }

            var pharmacyNames = roles.Select(r => r.Pharmacy.Name).Distinct().ToList();
            department = string.Join("|", pharmacyNames);
        }
        else
        {
            // If they have no active roles, we can clear the job title and department
            // In case of deactivation, PharmacyStaffDeactivationService will also override this with a 'disabled' string
            jobTitle = "";
            department = "";
        }

        if (!string.IsNullOrEmpty(partyDetails.Upn))
        {
            var userUpdate = new User
            {
                JobTitle = jobTitle,
                Department = department,
                OfficeLocation = licenceNumber,
                UsageLocation = "CA"
            };

            var success = await this.bcProviderClient.UpdateUser(partyDetails.Upn, userUpdate);
            if (success)
            {
                var details = $"JobTitle: {jobTitle}, Department: {department}, OfficeLocation: {licenceNumber}";
                this.context.BusinessEvents.Add(BCProviderAttributesUpdated.Create(partyId, details, this.clock.GetCurrentInstant()));
                await this.context.SaveChangesAsync(cancellationToken);
            }
        }

        // Sync Keycloak roles
        var allImmsBcRoles = new[]
        {
            MohKeycloakEnrolment.ImmsBcPhaAdmin,
            MohKeycloakEnrolment.ImmsBcPhaClinician,
            MohKeycloakEnrolment.ImmsBcPhaClerk
        };

        foreach (var enrolment in allImmsBcRoles)
        {
            if (enrolment == keycloakEnrolmentToAssign)
            {
                await this.keycloakClient.AssignAccessRoles(partyDetails.PrimaryUserId, enrolment);
            }
            else
            {
                await this.keycloakClient.RemoveAccessRoles(partyDetails.PrimaryUserId, enrolment);
            }
        }
    }
}
