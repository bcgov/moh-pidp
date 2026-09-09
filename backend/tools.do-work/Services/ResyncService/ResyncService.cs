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
using Pidp.Models;

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
            .AsSplitQuery()
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
                .Select(upn => upn!)
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

                await this.SyncBCProviderUpnsAsync(bcProviderUpns, additionalData, dryRun);
            }

            // Sync Keycloak
            foreach (var userId in party.Credentials.Select(c => c.UserId))
            {
                var ctx = new KeycloakSyncContext
                {
                    UserId = userId,
                    Party = party,
                    KeysToRemove = keysToRemove,
                    PlrStanding = plrStanding,
                    IsMd = isMd,
                    IsMoa = isMoa,
                    IsPharm = isPharm,
                    IsRnp = isRnp,
                    DryRun = dryRun,
                    LicenceStatusClientId = licenceStatusClientId,
                    EformsClientId = eformsClientId,
                    MdRole = mdRole,
                    MoaRole = moaRole,
                    PharmRole = pharmRole,
                    RnpRole = rnpRole,
                    SaRole = saRole,
                    ImmsRole = immsRole,
                    InfantRole = infantRole,
                    NpdpRole = npdpRole
                };
                await this.SyncKeycloakUserAsync(ctx);
            }
        }

        Console.WriteLine($"--- Resync Complete ({count} parties processed) ---");
    }

    private async Task SyncBCProviderUpnsAsync(List<string> bcProviderUpns, Dictionary<string, object> additionalData, bool dryRun)
    {
        foreach (var upn in bcProviderUpns.Where(u => !string.IsNullOrWhiteSpace(u)))
        {
            await this.SyncSingleBCProviderUpnAsync(upn, additionalData, dryRun);
        }
    }

    private async Task SyncSingleBCProviderUpnAsync(string upn, Dictionary<string, object> additionalData, bool dryRun)
    {
        var currentAttributes = await this.bcProviderClient.GetUserAttributes(upn, additionalData.Keys.ToArray());
        var hasChanges = currentAttributes == null;

        if (hasChanges)
        {
            this.logger.LogWarning("UPN {Upn} Could not retrieve current attributes from Entra.", upn);
        }
        else
        {
            foreach (var kvp in additionalData)
            {
                var newValueString = kvp.Value?.ToString()?.ToLowerInvariant() ?? "null";
                var currentValueString = currentAttributes!.TryGetValue(kvp.Key, out var currVal) ? (currVal?.ToString()?.ToLowerInvariant() ?? "null") : "null";

                if (newValueString != currentValueString)
                {
                    this.logger.LogInformation("UPN {Upn} Attribute {Key} changing from {CurrentValueString} to {NewValueString}", upn, kvp.Key, currentValueString, newValueString);
                    hasChanges = true;
                }
            }
        }

        if (hasChanges && !dryRun)
        {
            await this.bcProviderClient.UpdateAttributes(upn, additionalData);
        }
    }

    private async Task SyncKeycloakUserAsync(KeycloakSyncContext ctx)
    {
        var user = await this.keycloakClient.GetUser(ctx.UserId);
        if (user == null)
        {
            this.logger.LogWarning("Keycloak User ID {UserId} not found.", ctx.UserId);
            return;
        }

        user.Attributes ??= new Dictionary<string, string[]>();
        var requiresKeycloakUpdate = false;

        // Cleanup obsolete keys
        foreach (var key in ctx.KeysToRemove.Where(k => user.Attributes.ContainsKey(k)))
        {
            user.Attributes.Remove(key);
            requiresKeycloakUpdate = true;
            this.logger.LogInformation("Keycloak User ID {UserId}: Removing obsolete key '{Key}'", ctx.UserId, key);
        }

        // Add/Update new keys
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "practitionerrole", new[] { JsonSerializer.Serialize(ctx.PlrStanding.ProviderRoleTypes.Select(t => t.ToString())) });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "collegeid", new[] { JsonSerializer.Serialize(ctx.PlrStanding.CollegeIds) });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "msp_id", new[] { JsonSerializer.Serialize(ctx.PlrStanding.MspIds) });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "common_provider_number", new[] { ctx.Party.Cpn });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_md", new[] { ctx.IsMd.ToString() });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_moa", new[] { ctx.IsMoa.ToString() });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_pharm", new[] { ctx.IsPharm.ToString() });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_rnp", new[] { ctx.IsRnp.ToString() });

        if (requiresKeycloakUpdate)
        {
            if (!ctx.DryRun)
            {
                var success = await this.keycloakClient.UpdateUser(ctx.UserId, user);
                if (!success)
                {
                    this.logger.LogError("Failed to update Keycloak User ID {UserId}", ctx.UserId);
                }
            }
            else
            {
                this.logger.LogInformation("[DRY RUN] Would update Keycloak User ID {UserId}", ctx.UserId);
            }
        }

        if (!ctx.DryRun)
        {
            await this.SyncClientRoleAsync(ctx.UserId, ctx.LicenceStatusClientId, "MD", ctx.MdRole, ctx.IsMd);
            await this.SyncClientRoleAsync(ctx.UserId, ctx.LicenceStatusClientId, "MOA", ctx.MoaRole, ctx.IsMoa);
            await this.SyncClientRoleAsync(ctx.UserId, ctx.LicenceStatusClientId, "PHARM", ctx.PharmRole, ctx.IsPharm);
            await this.SyncClientRoleAsync(ctx.UserId, ctx.LicenceStatusClientId, "RNP", ctx.RnpRole, ctx.IsRnp);

            var hasSaEforms = ctx.Party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.SAEforms);
            var hasImms = ctx.Party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.ImmsBCEforms);
            var hasInfant = ctx.Party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.InfantRsvEforms);
            var hasNpdp = ctx.Party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.NpdpEforms);

            await this.SyncClientRoleAsync(ctx.UserId, ctx.EformsClientId, "phsa_eforms_sat", ctx.SaRole, hasSaEforms);
            await this.SyncClientRoleAsync(ctx.UserId, ctx.EformsClientId, "phsa_eforms_imms", ctx.ImmsRole, hasImms);
            await this.SyncClientRoleAsync(ctx.UserId, ctx.EformsClientId, "phsa_eforms_infant_rsv", ctx.InfantRole, hasInfant);
            await this.SyncClientRoleAsync(ctx.UserId, ctx.EformsClientId, "phsa_eforms_npdp", ctx.NpdpRole, hasNpdp);
        }
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

    private class KeycloakSyncContext
    {
        public Guid UserId { get; set; }
        public Party Party { get; set; } = null!;
        public string[] KeysToRemove { get; set; } = Array.Empty<string>();
        public PlrStandingsDigest PlrStanding { get; set; } = null!;
        public bool IsMd { get; set; }
        public bool IsMoa { get; set; }
        public bool IsPharm { get; set; }
        public bool IsRnp { get; set; }
        public bool DryRun { get; set; }
        public string LicenceStatusClientId { get; set; } = string.Empty;
        public string EformsClientId { get; set; } = string.Empty;
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? MdRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? MoaRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? PharmRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? RnpRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? SaRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? ImmsRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? InfantRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? NpdpRole { get; set; }
    }

    private static bool SetKeycloakAttribute(Pidp.Infrastructure.HttpClients.Keycloak.UserRepresentation user, string key, IEnumerable<string> EnumerableNewValue)
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
            await this.keycloakClient.GetClient("SAT-EFORMS");
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
