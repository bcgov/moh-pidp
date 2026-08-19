namespace DoWork.Services.DataDriftFixService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pidp;
using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

public class DataDriftFixService(
    PidpDbContext context,
    IPlrClient plrClient,
    IBCProviderClient bcProviderClient,
    PidpConfiguration config,
    ILogger<DataDriftFixService> logger) : IDataDriftFixService
{
    private readonly PidpDbContext context = context;
    private readonly IPlrClient plrClient = plrClient;
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly PidpConfiguration config = config;
    private readonly ILogger<DataDriftFixService> logger = logger;

    public async Task FixDataDriftAsync()
    {
        var credentials = await this.context.Credentials
            .Include(c => c.Party)
            .Where(c => c.IdentityProvider == IdentityProviders.BCProvider)
            .ToListAsync();

        var clientId = this.config.BCProviderClient.ClientId;
        var prefix = $"extension_{clientId.Replace("-", "")}_";
        var attributeNames = new[]
        {
            $"{prefix}isMd",
            $"{prefix}isMoa",
            $"{prefix}isPharm",
            $"{prefix}isRnp",
            $"{prefix}mspId",
            $"{prefix}endorserData"
        };

        var discrepancies = new Dictionary<string, IDictionary<string, object>>();
        var count = 0;

        Console.WriteLine($"Found {credentials.Count} BCProvider credentials to evaluate.");

        foreach (var credential in credentials)
        {
            count++;
            if (string.IsNullOrEmpty(credential.Party?.Cpn))
            {
                continue;
            }

            var upn = credential.IdpId;
            if (string.IsNullOrEmpty(upn))
            {
                continue;
            }

            Console.WriteLine($"[{count}/{credentials.Count}] Evaluating {upn}...");

            var plrStanding = await this.plrClient.GetStandingsDigestAsync(credential.Party.Cpn);
            
            var endorsingCpns = await this.context.ActiveEndorsingParties(credential.PartyId)
                .Select(party => party.Cpn)
                .ToListAsync();
            var endorsingPlrDigest = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

            var expectedIsMd = plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding;
            var expectedIsPharm = plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding;
            var expectedIsRnp = plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding;
            var expectedIsMoa = !plrStanding.HasGoodStanding && endorsingPlrDigest.HasGoodStanding;
            
            var expectedMspIds = plrStanding.MspIds;
            var expectedMspIdsString = expectedMspIds.Any() ? "[" + string.Join(",", expectedMspIds.Select(s => $"\"{s}\"")) + "]" : null;

            var expectedEndorserDataList = endorsingPlrDigest.WithGoodStanding().With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes).Cpns;
            var expectedEndorserDataString = "[" + string.Join(",", expectedEndorserDataList.Select(s => $"\"{s}\"")) + "]";

            var actualAttributes = await this.bcProviderClient.GetUserAttributes(upn, attributeNames);

            if (actualAttributes == null)
            {
                Console.WriteLine($"ERROR: Failed to retrieve attributes for {upn}");
                continue;
            }

            var actualIsMd = GetBoolValue(actualAttributes, $"{prefix}isMd");
            var actualIsPharm = GetBoolValue(actualAttributes, $"{prefix}isPharm");
            var actualIsRnp = GetBoolValue(actualAttributes, $"{prefix}isRnp");
            var actualIsMoa = GetBoolValue(actualAttributes, $"{prefix}isMoa");
            var actualMspIds = GetStringValue(actualAttributes, $"{prefix}mspId");
            var actualEndorserData = GetStringValue(actualAttributes, $"{prefix}endorserData");

            var updateAttributes = new BCProviderAttributes(clientId);
            var driftDetected = false;
            
            if (expectedIsMd != actualIsMd) 
            { 
                updateAttributes.SetIsMd(expectedIsMd); 
                Console.WriteLine($" - isMd drift: expected {expectedIsMd}, actual {actualIsMd}"); 
                driftDetected = true; 
            }
            if (expectedIsPharm != actualIsPharm) 
            { 
                updateAttributes.SetIsPharm(expectedIsPharm); 
                Console.WriteLine($" - isPharm drift: expected {expectedIsPharm}, actual {actualIsPharm}"); 
                driftDetected = true; 
            }
            if (expectedIsRnp != actualIsRnp) 
            { 
                updateAttributes.SetIsRnp(expectedIsRnp); 
                Console.WriteLine($" - isRnp drift: expected {expectedIsRnp}, actual {actualIsRnp}"); 
                driftDetected = true; 
            }
            if (expectedIsMoa != actualIsMoa) 
            { 
                updateAttributes.SetIsMoa(expectedIsMoa); 
                Console.WriteLine($" - isMoa drift: expected {expectedIsMoa}, actual {actualIsMoa}"); 
                driftDetected = true; 
            }
            if (expectedMspIdsString != actualMspIds) 
            { 
                updateAttributes.SetMspId(expectedMspIds);
                Console.WriteLine($" - mspId drift: expected {expectedMspIdsString ?? "null"}, actual {actualMspIds ?? "null"}"); 
                driftDetected = true; 
            }
            if (expectedEndorserDataString != (actualEndorserData ?? "[]")) 
            { 
                updateAttributes.SetEndorserData(expectedEndorserDataList); 
                Console.WriteLine($" - endorserData drift: expected {expectedEndorserDataString}, actual {actualEndorserData ?? "[]"}"); 
                driftDetected = true; 
            }

            if (driftDetected)
            {
                discrepancies.Add(upn, updateAttributes.AsAdditionalData());
            }
        }

        if (discrepancies.Count == 0)
        {
            Console.WriteLine("No configuration drift detected.");
            return;
        }

        Console.WriteLine($"\nFound {discrepancies.Count} credentials with configuration drift.");
        Console.WriteLine("Do you want to execute updates against Entra/BCProvider to fix these? (Y/N)");
        var input = Console.ReadLine();
        
        if (input?.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase) == true)
        {
            Console.WriteLine("Applying fixes...");
            foreach (var kvp in discrepancies)
            {
                var upn = kvp.Key;
                var updates = kvp.Value;
                var success = await this.bcProviderClient.UpdateAttributes(upn, updates);
                if (success)
                {
                    Console.WriteLine($"Successfully updated {upn}");
                }
                else
                {
                    Console.WriteLine($"Failed to update {upn}");
                }
            }
            Console.WriteLine("Fixes applied.");
        }
        else
        {
            Console.WriteLine("Aborting. No updates executed.");
        }
    }

    private static bool GetBoolValue(IDictionary<string, object> dict, string key)
    {
        if (dict.TryGetValue(key, out var val))
        {
            if (val is bool b) return b;
            if (val is string s && bool.TryParse(s, out var pb)) return pb;
        }
        return false;
    }

    private static string? GetStringValue(IDictionary<string, object> dict, string key)
    {
        if (dict.TryGetValue(key, out var val))
        {
            return val?.ToString();
        }
        return null;
    }
}
