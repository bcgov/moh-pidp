namespace DoWork.Services.ResyncService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Pidp;
using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

public interface IResyncService
{
    Task SynchronizeAsync(bool dryRun);
}

public class ResyncService(
    PidpDbContext context,
    IPlrClient plrClient,
    IBCProviderClient bcProviderClient,
    IKeycloakAdministrationClient keycloakClient,
    PidpConfiguration config,
    ILogger<ResyncService> logger) : IResyncService
{
    private readonly PidpDbContext context = context;
    private readonly IPlrClient plrClient = plrClient;
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly PidpConfiguration config = config;
    private readonly ILogger<ResyncService> logger = logger;

    public async Task SynchronizeAsync(bool dryRun)
    {
        Console.WriteLine("--- Starting Resync Service ---");
        
        if (dryRun)
        {
            Console.WriteLine("DRY RUN MODE: No updates will be applied to Entra or Keycloak.");
        } 
        else
        {
            Console.Write("ARE YOU SURE YOU WANT TO PROCEED WITH THE RESYNC SERVICE? (yes/no): ");
            var response = Console.ReadLine()?.ToLowerInvariant();
            if (response != "yes")
            {
                Console.WriteLine("Resync service aborted.");
                return;
            }
        }

        if (!await this.RunConnectivityChecksAsync())
        {
            Console.WriteLine("Connectivity checks failed. Aborting.");
            return;
        }

        var clientId = this.config.BCProviderClient.ClientId;
        var keysToRemove = new[] { "college_license_info", "college_licence_info", "college_certification_info" };

        // Get all parties' CPN from PIDP Database
        var parties = await this.context.Parties
            .Include(party => party.Credentials)
            .Include(party => party.AccessRequests)
            .Where(party => party.Cpn != null)
            .ToListAsync();

        Console.WriteLine($"Found {parties.Count} parties with a CPN to synchronize.");

        var licenceStatusClientId = "LICENCE-STATUS";
        var mdRole = await this.keycloakClient.GetClientRole(licenceStatusClientId, "MD");
        var moaRole = await this.keycloakClient.GetClientRole(licenceStatusClientId, "MOA");
        var pharmRole = await this.keycloakClient.GetClientRole(licenceStatusClientId, "PHARM");
        var rnpRole = await this.keycloakClient.GetClientRole(licenceStatusClientId, "RNP");

        var eformsClientId = "SAT-EFORMS";
        var saRole = await this.keycloakClient.GetClientRole(eformsClientId, "phsa_eforms_sat");
        var immsRole = await this.keycloakClient.GetClientRole(eformsClientId, "phsa_eforms_imms");
        var infantRole = await this.keycloakClient.GetClientRole(eformsClientId, "phsa_eforms_infant_rsv");
        var npdpRole = await this.keycloakClient.GetClientRole(eformsClientId, "phsa_eforms_npdp");

        var count = 0;
        foreach (var party in parties)
        {
            count++;
            if (count % 100 == 0)
            {
                Console.WriteLine($"Processed {count} / {parties.Count} parties...");
            }

            // Get PLR status
            var plrStanding = await this.plrClient.GetStandingsDigestAsync(party.Cpn);
            
            var endorsementRelations = await this.context.ActiveEndorsingParties(party.Id)
                .Select(p => p.Cpn)
                .ToListAsync();

            var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementRelations);

            var isMoa = !plrStanding.HasGoodStanding && endorsementPlrStanding.HasGoodStanding;
            var isMd = plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding;
            var isRnp = plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding;
            var isPharm = plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding;

            // Sync BCProvider
            var bcProviderUpns = party.Credentials
                .Where(c => c.IdentityProvider == IdentityProviders.BCProvider)
                .Select(c => c.IdpId)
                .Where(upn => upn != null)
                .ToList();

            if (bcProviderUpns.Count > 0)
            {
                var bcProviderAttributes = new BCProviderAttributes(clientId);
                bcProviderAttributes.SetIsMoa(isMoa);
                bcProviderAttributes.SetIsMd(isMd);
                bcProviderAttributes.SetIsRnp(isRnp);
                bcProviderAttributes.SetIsPharm(isPharm);
                bcProviderAttributes.SetMspId(plrStanding.MspIds);
                bcProviderAttributes.SetPractitionerRole(plrStanding.ProviderRoleTypes);
                bcProviderAttributes.SetCollegeId(plrStanding.CollegeIds);
                
                // Endorser data
                var endorserDataList = endorsementPlrStanding.WithGoodStanding().With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes).Cpns;
                bcProviderAttributes.SetEndorserData(endorserDataList);

                var additionalData = bcProviderAttributes.AsAdditionalData();

                foreach (var upn in bcProviderUpns)
                {
                    if (string.IsNullOrWhiteSpace(upn)) continue;

                    var currentAttributes = await this.bcProviderClient.GetUserAttributes(upn, additionalData.Keys.ToArray());
                    var hasChanges = false;
                    
                    if (currentAttributes != null)
                    {
                        foreach (var kvp in additionalData)
                        {
                            var newValueString = kvp.Value?.ToString()?.ToLowerInvariant() ?? "null";
                            var currentValueString = currentAttributes.TryGetValue(kvp.Key, out var currVal) ? (currVal?.ToString()?.ToLowerInvariant() ?? "null") : "null";

                            if (newValueString != currentValueString)
                            {
                                this.logger.LogInformation("UPN {Upn} Attribute {Key} changing from {CurrentValueString} to {NewValueString}", upn, kvp.Key, currentValueString, newValueString);
                                hasChanges = true;
                            }
                        }
                    }
                    else
                    {
                        this.logger.LogWarning("UPN {Upn} Could not retrieve current attributes from Entra.", upn);
                        hasChanges = true;
                    }

                    if (hasChanges && !dryRun)
                    {
                        await this.bcProviderClient.UpdateAttributes(upn, additionalData);
                    }
                }
            }

            // Sync Keycloak
            foreach (var credential in party.Credentials)
            {
                var user = await this.keycloakClient.GetUser(credential.UserId);
                if (user == null)
                {
                    this.logger.LogWarning("Keycloak User ID {UserId} not found.", credential.UserId);
                    continue;
                }

                user.Attributes ??= new Dictionary<string, string[]>();
                var requiresKeycloakUpdate = false;

                // Cleanup obsolete keys
                foreach (var key in keysToRemove)
                {
                    if (user.Attributes.ContainsKey(key))
                    {
                        user.Attributes.Remove(key);
                        requiresKeycloakUpdate = true;
                        this.logger.LogInformation("Keycloak User ID {UserId}: Removing obsolete key '{Key}'", credential.UserId, key);
                    }
                }

                // Add/Update new keys
                requiresKeycloakUpdate |= SetKeycloakAttribute(user, "practitionerrole", new[] { JsonSerializer.Serialize(plrStanding.ProviderRoleTypes.Select(t => t.ToString())) });
                requiresKeycloakUpdate |= SetKeycloakAttribute(user, "collegeid", new[] { JsonSerializer.Serialize(plrStanding.CollegeIds) });
                requiresKeycloakUpdate |= SetKeycloakAttribute(user, "msp_id", new[] { JsonSerializer.Serialize(plrStanding.MspIds) });
                requiresKeycloakUpdate |= SetKeycloakAttribute(user, "common_provider_number", new[] { party.Cpn });
                requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_md", new[] { isMd.ToString() });
                requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_moa", new[] { isMoa.ToString() });
                requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_pharm", new[] { isPharm.ToString() });
                requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_rnp", new[] { isRnp.ToString() });

                if (requiresKeycloakUpdate)
                {
                    if (!dryRun)
                    {
                        var success = await this.keycloakClient.UpdateUser(credential.UserId, user);
                        if (!success)
                        {
                            this.logger.LogError("Failed to update Keycloak User ID {UserId}", credential.UserId);
                        }
                    }
                    else
                    {
                        this.logger.LogInformation("[DRY RUN] Would update Keycloak User ID {UserId}", credential.UserId);
                    }
                }

                if (!dryRun)
                {
                    await this.SyncClientRoleAsync(credential.UserId, licenceStatusClientId, "MD", mdRole, isMd);
                    await this.SyncClientRoleAsync(credential.UserId, licenceStatusClientId, "MOA", moaRole, isMoa);
                    await this.SyncClientRoleAsync(credential.UserId, licenceStatusClientId, "PHARM", pharmRole, isPharm);
                    await this.SyncClientRoleAsync(credential.UserId, licenceStatusClientId, "RNP", rnpRole, isRnp);

                    var hasSaEforms = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.SAEforms);
                    var hasImms = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.ImmsBCEforms);
                    var hasInfant = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.InfantRsvEforms);
                    var hasNpdp = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.NpdpEforms);

                    await this.SyncClientRoleAsync(credential.UserId, eformsClientId, "phsa_eforms_sat", saRole, hasSaEforms);
                    await this.SyncClientRoleAsync(credential.UserId, eformsClientId, "phsa_eforms_imms", immsRole, hasImms);
                    await this.SyncClientRoleAsync(credential.UserId, eformsClientId, "phsa_eforms_infant_rsv", infantRole, hasInfant);
                    await this.SyncClientRoleAsync(credential.UserId, eformsClientId, "phsa_eforms_npdp", npdpRole, hasNpdp);
                }
            }
        }

        Console.WriteLine($"--- Resync Complete ({count} parties processed) ---");
    }

    private async Task SyncClientRoleAsync(Guid userId, string clientId, string roleName, Pidp.Infrastructure.HttpClients.Keycloak.Role? role, bool shouldHaveRole)
    {
        if (shouldHaveRole)
        {
            await this.keycloakClient.AssignClientRole(userId, clientId, roleName);
        }
        else if (role != null)
        {
            await this.keycloakClient.RemoveClientRole(userId, role);
        }
    }

    private static bool SetKeycloakAttribute(Pidp.Infrastructure.HttpClients.Keycloak.UserRepresentation user, string key, IEnumerable<string?> EnumerableNewValue)
    {
        var newValueList = EnumerableNewValue.ToList();
        if (user.Attributes.TryGetValue(key, out var currentValue))
        {
            var currentValueList = currentValue.ToList();
            if (currentValueList.SequenceEqual(newValueList))
            {
                return false;
            }
        }
        else if (newValueList.Count == 0)
        {
            return false;
        }

        user.Attributes[key] = newValueList.ToArray();
        return true;
    }

    private async Task<bool> RunConnectivityChecksAsync()
    {
        Console.WriteLine("Running connectivity checks...");
        var allPassed = true;

        // 1. Database
        try
        {
            var canConnect = await this.context.Database.CanConnectAsync();
            Console.WriteLine($"Database: {(canConnect ? "PASS" : "FAIL")}");
            allPassed &= canConnect;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database: FAIL ({ex.Message})");
            allPassed = false;
        }

        // 2. PLR Webservice
        try
        {
            var plrTest = await this.plrClient.GetProcessableStatusChangesAsync(1);
            Console.WriteLine($"PLR Webservice: {(plrTest != null ? "PASS" : "FAIL")}");
            allPassed &= (plrTest != null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PLR Webservice: FAIL ({ex.Message})");
            allPassed = false;
        }

        // 3. Keycloak
        try
        {
            var kcTest = await this.keycloakClient.GetClient("SAT-EFORMS");
            Console.WriteLine("Keycloak: PASS");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Keycloak: FAIL ({ex.Message})");
            allPassed = false;
        }

        // 4. BCProvider (Entra ID)
        try
        {
            await this.bcProviderClient.GetUserAttributes("test-connection@example.com", Array.Empty<string>());
            Console.WriteLine("BCProvider (Entra ID): PASS");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"BCProvider: FAIL ({ex.Message})");
            allPassed = false;
        }

        return allPassed;
    }
}
