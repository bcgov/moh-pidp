namespace Pidp.Infrastructure.Services;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;

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
            await this.ProcessPartyDeactivationAsync(partyId, yesterdayStart, yesterdayEnd, cancellationToken);
            Thread.Sleep(5000); // Wait 5 seconds for BCProvider to update the user, be a nice neighbor
        }



        this.logger.LogInformation("Finished daily pharmacy staff deactivation task.");
    }

    private async Task ProcessPartyDeactivationAsync(int partyId, DateTime yesterdayStart, DateTime yesterdayEnd, CancellationToken cancellationToken)
    {
        try
        {
            var allRoles = await this.context.PharmacyPartyRoles
                .Where(r => r.PartyId == partyId)
                .ToListAsync(cancellationToken);

            var roleEndedYesterday = allRoles.FirstOrDefault(r => r.EffectiveEndDate >= yesterdayStart && r.EffectiveEndDate < yesterdayEnd);
            if (roleEndedYesterday == null)
            {
                return;
            }

            bool shouldDisable = allRoles.All(r => r.Id == roleEndedYesterday.Id || r.EffectiveEndDate != null && r.EffectiveEndDate < yesterdayEnd);

            if (!shouldDisable)
            {
                return;
            }

            var upn = await this.context.Credentials
                .Where(c => c.PartyId == partyId && c.IdentityProvider == IdentityProviders.BCProvider)
                .Select(c => c.IdpId)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrEmpty(upn))
            {
                return;
            }

#pragma warning disable CA1305
            var dateStr = roleEndedYesterday.EffectiveEndDate!.Value.ToString("yyyyMMdd");
#pragma warning restore CA1305
            var disabledJobTitle = $"disabled (onehealthid,immsbc,{roleEndedYesterday.PharmacyId},{dateStr})";

            var userUpdate = new User
            {
                JobTitle = disabledJobTitle
            };

            var success = await this.bcProviderClient.UpdateUser(upn, userUpdate);
            if (success)
            {
                if (this.logger.IsEnabled(LogLevel.Information))
                {
                    this.logger.LogInformation("Successfully set job title to '{JobTitle}' for user '{Upn}'.", disabledJobTitle, upn);
                }
            }
            else
            {
                this.logger.LogError("Failed to update job title for user '{Upn}'.", upn);
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error processing deactivation for PartyId {PartyId}.", partyId);
        }

        this.logger.LogInformation("Finished daily pharmacy staff deactivation task.");
    }
}
