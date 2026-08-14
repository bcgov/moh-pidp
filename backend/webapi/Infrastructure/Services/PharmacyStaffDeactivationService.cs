namespace Pidp.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class PharmacyStaffDeactivationService(
    PidpDbContext context,
    IBCProviderClient bcProviderClient,
    IClock clock,
    ILogger<PharmacyStaffDeactivationService> logger) : IPharmacyStaffDeactivationService
{
    private readonly PidpDbContext context = context;
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly IClock clock = clock;
    private readonly ILogger<PharmacyStaffDeactivationService> logger = logger;

    public async Task DeactivateExpiredStaffAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Starting daily pharmacy staff deactivation task.");

        var today = this.clock.GetCurrentInstant().ToDateTimeUtc().Date;
        var yesterdayStart = today.AddDays(-1);
        var yesterdayEnd = today;

        var partiesWithExpiredRoles = await this.context.PharmacyPartyRoles
            .Where(role => role.EffectiveEndDate >= yesterdayStart && role.EffectiveEndDate < yesterdayEnd)
            .Select(role => role.PartyId)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var partyId in partiesWithExpiredRoles)
        {
            try
            {
                var allRoles = await this.context.PharmacyPartyRoles
                    .Where(r => r.PartyId == partyId)
                    .ToListAsync(cancellationToken);

                var roleEndedYesterday = allRoles.FirstOrDefault(r => r.EffectiveEndDate >= yesterdayStart && r.EffectiveEndDate < yesterdayEnd);
                if (roleEndedYesterday == null)
                {
                    continue;
                }

                bool shouldDisable = true;
                foreach (var role in allRoles)
                {
                    if (role.Id == roleEndedYesterday.Id)
                    {
                        continue;
                    }

                    if (role.EffectiveEndDate == null || role.EffectiveEndDate >= yesterdayEnd)
                    {
                        shouldDisable = false;
                        break;
                    }
                }

                if (shouldDisable)
                {
                    var upn = await this.context.Credentials
                        .Where(c => c.PartyId == partyId && c.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(c => c.IdpId)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (!string.IsNullOrEmpty(upn))
                    {
                        var dateStr = roleEndedYesterday.EffectiveEndDate.Value.ToString("yyyyMMdd");
                        var disabledJobTitle = $"disabled (onehealthid,immsbc,{roleEndedYesterday.PharmacyId},{dateStr})";

                        var userUpdate = new User
                        {
                            JobTitle = disabledJobTitle
                        };

                        var success = await this.bcProviderClient.UpdateUser(upn, userUpdate);
                        if (success)
                        {
                            this.logger.LogInformation("Successfully set job title to '{jobTitle}' for user '{upn}'.", disabledJobTitle, upn);
                        }
                        else
                        {
                            this.logger.LogError("Failed to update job title for user '{upn}'.", upn);
                        }
                    }

                    Thread.Sleep(5 * 1000);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error processing deactivation for PartyId {partyId}.", partyId);
            }
        }

        this.logger.LogInformation("Finished daily pharmacy staff deactivation task.");
    }
}
