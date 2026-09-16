using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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

namespace DoWork.Services.ResyncService;

public class PartySyncSnapshot
{
    public int PartyId { get; set; }
    public Guid UserId { get; set; }
    public string? Cpn { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EndorsementSummary { get; set; }
    public DesiredState Expected { get; set; } = new();
    public ActualBCProviderState? BCProvider { get; set; }
    public ActualKeycloakState? Keycloak { get; set; }
}

public class DesiredState
{
    public bool IsMd { get; set; }
    public bool IsMoa { get; set; }
    public bool IsPharm { get; set; }
    public bool IsRnp { get; set; }
    public IEnumerable<string> MspIds { get; set; } = Array.Empty<string>();
    public IEnumerable<string> ProviderRoleTypes { get; set; } = Array.Empty<string>();
    public IEnumerable<string> CollegeIds { get; set; } = Array.Empty<string>();
    public IEnumerable<string> EndorserData { get; set; } = Array.Empty<string>();
    public string? OpId { get; set; }
    
    // Keycloak Roles
    public bool HasMdRole { get; set; }
    public bool HasMoaRole { get; set; }
    public bool HasPharmRole { get; set; }
    public bool HasRnpRole { get; set; }
    public bool HasSaRole { get; set; }
    public bool HasImmsRole { get; set; }
    public bool HasInfantRole { get; set; }
    public bool HasNpdpRole { get; set; }
}

public class ActualBCProviderState
{
    public string Upn { get; set; } = string.Empty;
    public IDictionary<string, object>? Attributes { get; set; }
}

public class ActualKeycloakState
{
    public Guid UserId { get; set; }
    public Dictionary<string, string[]> Attributes { get; set; } = new();
}

public class SnapshotComparisonResult
{
    public bool HasChanges { get; set; }
    public List<string> Anomalies { get; set; } = new();
}

public interface IResyncService
{
    Task SynchronizeAsync(bool dryRun, int? partyId = null);
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

    public async Task SynchronizeAsync(bool dryRun, int? partyId = null)
    {
        Console.WriteLine("--- Starting Resync Service ---");
        
        if (dryRun)
        {
            Console.WriteLine("DRY RUN MODE: No updates will be applied to Entra or Keycloak.");
        } 
        else
        {
            Console.WriteLine("LIVE RUN MODE: Updates will be applied to Entra and Keycloak.");
        }

        if (!await this.RunConnectivityChecksAsync())
        {
            Console.WriteLine("Connectivity checks failed. Aborting.");
            return;
        }

        var clientId = this.config.BCProviderClient.ClientId;
        var keysToRemove = new[] { "college_license_info", "college_licence_info", "college_certification_info" };

        var query = this.context.Parties
            .Include(party => party.Credentials)
            .Include(party => party.AccessRequests)
            .AsSplitQuery()
            .Where(party => party.Cpn != null 
                         || party.Credentials.Any(c => c.IdentityProvider == IdentityProviders.BCProvider)
                         || this.context.EndorsementRelationships.Any(er => er.PartyId == party.Id));

        if (partyId.HasValue)
        {
            query = query.Where(party => party.Id == partyId.Value);
        }

        var parties = await query.ToListAsync();

        Console.WriteLine($"Found {parties.Count} parties to synchronize.");

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

        // 1. Gather all CPNs
        var allCpns = parties.Select(p => p.Cpn).Where(c => c != null).Cast<string>().ToList();
        
        // Also fetch all endorsement relations upfront so we can get those CPNs
        Console.WriteLine("Fetching endorsement relationships...");
        var endorsementDictionary = new Dictionary<int, List<Party>>();
        foreach (var party in parties)
        {
            var relations = await this.context.ActiveEndorsingParties(party.Id).ToListAsync();
            endorsementDictionary[party.Id] = relations;
            
            var nonNullRelations = relations.Where(c => c.Cpn != null).Select(c => c.Cpn).Cast<string>().ToList();
            allCpns.AddRange(nonNullRelations);
        }

        allCpns = allCpns.Distinct().ToList();

        // 2. Fetch all PLR records
        Console.WriteLine($"Fetching PLR records for {allCpns.Count} CPNs...");
        var plrRecordsByCpn = new Dictionary<string, IEnumerable<PlrRecord>>();
        
        var chunkedCpns = allCpns.Chunk(50);
        foreach (var chunk in chunkedCpns)
        {
            var records = await this.plrClient.GetRecordsAsync(chunk.ToArray());
            if (records != null)
            {
                var grouped = records.GroupBy(r => r.Cpn);
                foreach (var group in grouped)
                {
                    if (!string.IsNullOrEmpty(group.Key))
                    {
                        plrRecordsByCpn[group.Key] = group.ToList();
                    }
                }
            }
        }

        var snapshots = new List<PartySyncSnapshot>();
        var count = 0;
        foreach (var party in parties)
        {
            count++;
            if (count % 100 == 0)
            {
                Console.WriteLine($"Processed {count} / {parties.Count} parties...");
            }

            // In memory digest calculation
            PlrStandingsDigest plrStanding;
            if (party.Cpn != null && plrRecordsByCpn.TryGetValue(party.Cpn, out var records))
            {
                plrStanding = PlrStandingsDigest.FromRecords(records);
            }
            else
            {
                plrStanding = PlrStandingsDigest.FromEmpty();
            }

            var endorsementRelations = endorsementDictionary[party.Id];
            var endorsementSummaries = new List<string>();
            var endorsementRecords = new List<PlrRecord>();
            
            foreach (var relation in endorsementRelations)
            {
                if (relation.Cpn != null && plrRecordsByCpn.TryGetValue(relation.Cpn, out var r))
                {
                    endorsementRecords.AddRange(r);
                    var standing = PlrStandingsDigest.FromRecords(r).WithGoodStanding().With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes);
                    if (standing.HasGoodStanding)
                    {
                        var identifierType = r.FirstOrDefault()?.IdentifierType;
                        var collegeId = r.FirstOrDefault()?.CollegeId;
                        endorsementSummaries.Add($"Endorsement: {relation.Id} {identifierType} {collegeId}");
                    }
                }
            }
            
            var endorsementSummaryStr = string.Join(", ", endorsementSummaries);
            if (string.IsNullOrEmpty(endorsementSummaryStr)) endorsementSummaryStr = "Endorsement: None";

            var endorsementPlrStanding = endorsementRecords.Any() ? PlrStandingsDigest.FromRecords(endorsementRecords) : PlrStandingsDigest.FromEmpty();

            var isMoa = !plrStanding.HasGoodStanding && endorsementPlrStanding.HasGoodStanding;
            var isMd = plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding;
            var isRnp = plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding;
            var isPharm = plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding;

            if (string.IsNullOrEmpty(party.OpId) && !dryRun)
            {
                await party.GenerateOpId(this.context);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation("Generated OpId {OpId} for Party {PartyId}", party.OpId, party.Id);
            }

            var desired = new DesiredState
            {
                IsMd = isMd,
                IsMoa = isMoa,
                IsPharm = isPharm,
                IsRnp = isRnp,
                MspIds = plrStanding.MspIds ?? Array.Empty<string>(),
                ProviderRoleTypes = plrStanding.ProviderRoleTypes.Select(x => x.ToString()).ToList(),
                CollegeIds = plrStanding.CollegeIds ?? Array.Empty<string>(),
                EndorserData = endorsementPlrStanding.WithGoodStanding().With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes).Cpns ?? Array.Empty<string>(),
                OpId = party.OpId,
                HasMdRole = isMd,
                HasMoaRole = isMoa,
                HasPharmRole = isPharm,
                HasRnpRole = isRnp,
                HasSaRole = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.SAEforms),
                HasImmsRole = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.ImmsBCEforms),
                HasInfantRole = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.InfantRsvEforms),
                HasNpdpRole = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.NpdpEforms)
            };

            var primaryUserId = party.Credentials.FirstOrDefault()?.UserId ?? Guid.Empty;

            var snapshot = new PartySyncSnapshot
            {
                PartyId = party.Id,
                UserId = primaryUserId,
                Cpn = party.Cpn,
                FirstName = party.FirstName,
                LastName = party.LastName,
                EndorsementSummary = endorsementSummaryStr,
                Expected = desired
            };

            // Fetch BCProvider State
            var bcProviderUpns = party.Credentials
                .Where(c => c.IdentityProvider == IdentityProviders.BCProvider)
                .Select(c => c.IdpId)
                .Where(upn => upn != null)
                .Select(upn => upn!)
                .ToList();

            if (bcProviderUpns.Count > 0)
            {
                try
                {
                    var upn = bcProviderUpns.Single();
                    var attributes = await this.bcProviderClient.GetUserAttributes(upn, new BCProviderAttributes(clientId).AsAdditionalData().Keys.ToArray());
                    snapshot.BCProvider = new ActualBCProviderState
                    {
                        Upn = upn,
                        Attributes = attributes
                    };
                }
                catch (InvalidOperationException)
                {
                    this.logger.LogError("Party {PartyId} has multiple BCProvider credentials! Cannot fetch a single BCProvider actual state.", party.Id);
                }
            }

            // Fetch Keycloak State
            if (primaryUserId != Guid.Empty)
            {
                var kcUser = await this.keycloakClient.GetUser(primaryUserId);
                if (kcUser != null)
                {
                    snapshot.Keycloak = new ActualKeycloakState
                    {
                        UserId = primaryUserId,
                        Attributes = kcUser.Attributes ?? new Dictionary<string, string[]>()
                    };
                }
            }

            snapshots.Add(snapshot);
            
            // Compare and log
            CompareAndLog(snapshot);

            // Apply updates
            if (!dryRun)
            {
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
                    bcProviderAttributes.SetEndorserData(desired.EndorserData);
                    if (!string.IsNullOrEmpty(party.OpId))
                    {
                        bcProviderAttributes.SetOpId(party.OpId);
                    }

                    var additionalData = bcProviderAttributes.AsAdditionalData();
                    foreach (var upn in bcProviderUpns.Where(u => !string.IsNullOrWhiteSpace(u)))
                    {
                        await this.SyncSingleBCProviderUpnAsync(upn, additionalData, dryRun);
                    }
                }

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
        }

        // Write JSON representation
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var filename = $"output/resync_{timestamp}.json";
        var jsonContent = JsonSerializer.Serialize(snapshots, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filename, jsonContent);
        Console.WriteLine($"Wrote JSON output to {filename}");

        Console.WriteLine($"--- Resync Complete ({count} parties processed) ---");
    }

    private static void CompareAndLog(PartySyncSnapshot snapshot)
    {
        var expected = snapshot.Expected;
        var licenses = expected.CollegeIds.Any() ? string.Join(",", expected.CollegeIds) : "None";
        var roles = expected.ProviderRoleTypes.Any() ? string.Join(",", expected.ProviderRoleTypes) : "None";

        Console.WriteLine($"Party: {snapshot.PartyId}, {snapshot.LastName}, {snapshot.FirstName}, Licenses: {licenses}, PractitionerRole: {roles}, {snapshot.EndorsementSummary}");

        var hasAnomalies = false;
        
        // Helper
        void Check(string prop, string expectedStr, string actualStr)
        {
            if (expectedStr != actualStr)
            {
                hasAnomalies = true;
                if (string.IsNullOrEmpty(actualStr) || actualStr == "null")
                {
                    Console.WriteLine($"    {prop}: Unset -> {expectedStr}");
                }
                else
                {
                    Console.WriteLine($"    {prop}: {actualStr} -> {expectedStr}");
                }
            }
        }

        // Compare BCProvider
        if (snapshot.BCProvider?.Attributes != null)
        {
            var attrs = snapshot.BCProvider.Attributes;
            string GetBcpValue(string suffix) 
            {
                var key = attrs.Keys.FirstOrDefault(k => k.EndsWith(suffix, StringComparison.OrdinalIgnoreCase));
                return (key != null && attrs.TryGetValue(key, out var val)) ? (val?.ToString()?.ToLower() ?? "null") : "null";
            }

            Check("BCProvider IsMoa", snapshot.Expected.IsMoa.ToString().ToLower(), GetBcpValue("_isMoa"));
            Check("BCProvider IsMd", snapshot.Expected.IsMd.ToString().ToLower(), GetBcpValue("_isMd"));
            Check("BCProvider IsPharm", snapshot.Expected.IsPharm.ToString().ToLower(), GetBcpValue("_isPharm"));
            Check("BCProvider IsRnp", snapshot.Expected.IsRnp.ToString().ToLower(), GetBcpValue("_isRnp"));
            Check("BCProvider OpId", snapshot.Expected.OpId?.ToLower() ?? "null", GetBcpValue("_opId"));

            string ArrayToStr(IEnumerable<string> arr) => ("[" + string.Join(",", arr.Select(s => $"\"{s}\"")) + "]").ToLower();

            Check("BCProvider CollegeId", ArrayToStr(snapshot.Expected.CollegeIds), GetBcpValue("_collegeid"));
            Check("BCProvider MspId", ArrayToStr(snapshot.Expected.MspIds), GetBcpValue("_mspId"));
            Check("BCProvider PractitionerRole", ArrayToStr(snapshot.Expected.ProviderRoleTypes), GetBcpValue("_practitionerRole"));
            Check("BCProvider EndorserData", ArrayToStr(snapshot.Expected.EndorserData), GetBcpValue("_endorserData"));
        }

        // Compare Keycloak
        if (snapshot.Keycloak != null)
        {
            var attrs = snapshot.Keycloak.Attributes;
            string GetKcValue(string key) => attrs.GetValueOrDefault(key)?.FirstOrDefault()?.ToLower() ?? "null";

            Check("Keycloak is_moa", snapshot.Expected.IsMoa.ToString().ToLower(), GetKcValue("is_moa"));
            Check("Keycloak is_md", snapshot.Expected.IsMd.ToString().ToLower(), GetKcValue("is_md"));
            Check("Keycloak is_pharm", snapshot.Expected.IsPharm.ToString().ToLower(), GetKcValue("is_pharm"));
            Check("Keycloak is_rnp", snapshot.Expected.IsRnp.ToString().ToLower(), GetKcValue("is_rnp"));
            Check("Keycloak opId", snapshot.Expected.OpId?.ToLower() ?? "null", GetKcValue("opId"));
        }
        
        if (!hasAnomalies)
        {
            Console.WriteLine("    No changes required.");
        }
    }

    // Keep the other existing methods below...
    
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

        foreach (var key in ctx.KeysToRemove.Where(k => user.Attributes.ContainsKey(k)))
        {
            user.Attributes.Remove(key);
            requiresKeycloakUpdate = true;
            this.logger.LogInformation("Keycloak User ID {UserId}: Removing obsolete key '{Key}'", ctx.UserId, key);
        }

        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "practitionerrole", new[] { JsonSerializer.Serialize(ctx.PlrStanding.ProviderRoleTypes.Select(t => t.ToString())) });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "collegeid", new[] { JsonSerializer.Serialize(ctx.PlrStanding.CollegeIds) });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "msp_id", new[] { JsonSerializer.Serialize(ctx.PlrStanding.MspIds) });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "common_provider_number", new[] { ctx.Party.Cpn });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_md", new[] { ctx.IsMd.ToString() });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_moa", new[] { ctx.IsMoa.ToString() });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_pharm", new[] { ctx.IsPharm.ToString() });
        requiresKeycloakUpdate |= SetKeycloakAttribute(user, "is_rnp", new[] { ctx.IsRnp.ToString() });

        if (!string.IsNullOrEmpty(ctx.Party.OpId))
        {
            requiresKeycloakUpdate |= SetKeycloakAttribute(user, "opId", new[] { ctx.Party.OpId });
        }

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
