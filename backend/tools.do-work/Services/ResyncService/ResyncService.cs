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
using Pidp.Models;
using Pidp.Models.Lookups;

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
    public List<PartyCredentialSnapshot> Credentials { get; set; } = new();
}

public class PartyCredentialSnapshot
{
    public string IdentityProvider { get; set; } = string.Empty;
    public string IdpId { get; set; } = string.Empty;
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

    private sealed record KeycloakRolesContext(
        Pidp.Infrastructure.HttpClients.Keycloak.Role? SaRole,
        Pidp.Infrastructure.HttpClients.Keycloak.Role? ImmsRole,
        Pidp.Infrastructure.HttpClients.Keycloak.Role? InfantRole,
        Pidp.Infrastructure.HttpClients.Keycloak.Role? NpdpRole
    );

    private sealed record GlobalSyncContext(
        bool DryRun,
        Dictionary<string, IEnumerable<PlrRecord>> PlrRecordsByCpn,
        KeycloakRolesContext KeycloakRoles,
        Dictionary<int, List<Party>> EndorsementDictionary,
        List<EformsDrift> EformsDrifts,
        List<PartyChangeLog> ChangeLogs
    );

    private class PartyChangeLog
    {
        public string PartyHeader { get; set; } = string.Empty;
        public string? EndorserData { get; set; }
        public List<string>? BCProviderChanges { get; set; }
        public List<string>? KeycloakChanges { get; set; }
    }

    private class EformsDrift
    {
        public int PartyId { get; set; }
        public string? Add { get; set; }
        public string? Remove { get; set; }
    }

    public async Task SynchronizeAsync(bool dryRun, int? partyId = null)
    {
        Console.WriteLine("--- Starting Resync Service ---");
        
        if (dryRun) Console.WriteLine("DRY RUN MODE: No updates will be applied to Entra or Keycloak.");
        else Console.WriteLine("LIVE RUN MODE: Updates will be applied to Entra and Keycloak.");

        if (!await this.RunConnectivityChecksAsync())
        {
            Console.WriteLine("Connectivity checks failed. Aborting.");
            return;
        }

        var parties = await this.FetchPartiesAsync(partyId);
        if (parties.Count == 0) return;

        var rolesContext = await this.FetchKeycloakRolesAsync();
        var endorsementDictionary = await this.FetchEndorsementRelationshipsAsync(parties);
        var plrRecordsByCpn = await this.FetchPlrRecordsAsync(parties, endorsementDictionary);

        var syncContext = new GlobalSyncContext(dryRun, plrRecordsByCpn, rolesContext, endorsementDictionary, new List<EformsDrift>(), new List<PartyChangeLog>());
        
        var snapshots = new List<PartySyncSnapshot>();
        var exceptions = new List<string>();
        var count = 0;
        foreach (var party in parties)
        {
            count++;
            if (count % 100 == 0)
            {
                Console.WriteLine($"Processed {count} / {parties.Count} parties...");
            }

            try
            {
                var snapshot = await this.ProcessPartyAsync(party, syncContext);
                if (snapshot != null)
                {
                    snapshots.Add(snapshot);
                }
            }
            catch (Exception ex)
            {
                var msg = $"Exception for PartyId {party.Id}: {ex.Message}";
                Console.WriteLine(msg);
                exceptions.Add(msg + Environment.NewLine + ex.StackTrace);
            }
        }

        Console.WriteLine("");
        await this.WriteJsonOutputAsync(snapshots);
        await this.WriteEformsDriftOutputAsync(syncContext.EformsDrifts);
        await this.WriteChangesOutputAsync(syncContext.ChangeLogs);
        
        if (exceptions.Count > 0)
        {
            await this.WriteExceptionsOutputAsync(exceptions);
        }

        Console.WriteLine($"--- Resync Complete ({count} parties processed) ---");
    }

    private async Task<List<Party>> FetchPartiesAsync(int? partyId)
    {
        var query = this.context.Parties
            .Include(party => party.Credentials)
            .Include(party => party.AccessRequests)
            .AsSplitQuery()
            .Where(party => party.Cpn != null 
                         || party.Credentials.Any(c => c.IdentityProvider == IdentityProviders.BCProvider)
                         || this.context.EndorsementRelationships.Any(er => er.PartyId == party.Id));

        if (partyId.HasValue) query = query.Where(party => party.Id == partyId.Value);

        var parties = await query.ToListAsync();
        Console.WriteLine($"Found {parties.Count} parties to synchronize.");
        return parties;
    }

    private async Task<KeycloakRolesContext> FetchKeycloakRolesAsync()
    {
        var eformsClientId = "SAT-EFORMS";

        return new KeycloakRolesContext(
            await this.keycloakClient.GetClientRole(eformsClientId, "phsa_eforms_sat"),
            await this.keycloakClient.GetClientRole(eformsClientId, "phsa_eforms_imms"),
            await this.keycloakClient.GetClientRole(eformsClientId, "phsa_eforms_infant_rsv"),
            await this.keycloakClient.GetClientRole(eformsClientId, "phsa_eforms_npdp")
        );
    }

    private async Task<Dictionary<int, List<Party>>> FetchEndorsementRelationshipsAsync(List<Party> parties)
    {
        Console.WriteLine("Fetching endorsement relationships...");
        var dictionary = new Dictionary<int, List<Party>>();
        foreach (var party in parties)
        {
            dictionary[party.Id] = await this.context.ActiveEndorsingParties(party.Id).ToListAsync();
        }
        return dictionary;
    }

    private async Task<Dictionary<string, IEnumerable<PlrRecord>>> FetchPlrRecordsAsync(List<Party> parties, Dictionary<int, List<Party>> endorsementDictionary)
    {
        var allCpns = parties.Select(p => p.Cpn).Where(c => c != null).Cast<string>().ToList();
        
        foreach (var relation in endorsementDictionary.Values)
        {
            allCpns.AddRange(relation.Where(c => c.Cpn != null).Select(c => c.Cpn).Cast<string>());
        }

        allCpns = allCpns.Distinct().ToList();

        Console.WriteLine($"Fetching PLR records for {allCpns.Count} CPNs...");
        var plrRecordsByCpn = new Dictionary<string, IEnumerable<PlrRecord>>();
        
        foreach (var chunk in allCpns.Chunk(50))
        {
            var records = await this.plrClient.GetRecordsAsync(chunk.ToArray());
            if (records != null)
            {
                foreach (var group in records.GroupBy(r => r.Cpn).Where(g => !string.IsNullOrEmpty(g.Key)))
                {
                    plrRecordsByCpn[group.Key] = group.ToList();
                }
            }
        }
        return plrRecordsByCpn;
    }

    private async Task<PartySyncSnapshot> ProcessPartyAsync(Party party, GlobalSyncContext ctx)
    {
        var plrStanding = CalculatePlrStanding(party.Cpn, ctx.PlrRecordsByCpn);
        var (endorsementSummaryStr, endorsementPlrStanding) = CalculateEndorsements(party, ctx.EndorsementDictionary, ctx.PlrRecordsByCpn);

        var desired = CalculateDesiredState(party, plrStanding, endorsementPlrStanding);

        var primaryUserId = party.Credentials.FirstOrDefault()?.UserId ?? Guid.Empty;

        var snapshot = new PartySyncSnapshot
        {
            PartyId = party.Id,
            UserId = primaryUserId,
            Cpn = party.Cpn,
            FirstName = party.FirstName,
            LastName = party.LastName,
            EndorsementSummary = endorsementSummaryStr,
            Expected = desired,
            Credentials = party.Credentials.Select(c => new PartyCredentialSnapshot 
            { 
                IdentityProvider = c.IdentityProvider, 
                IdpId = c.IdpId ?? "Unknown"
            }).ToList()
        };

        var bcProviderUpns = GetBCProviderUpns(party);
        await FetchActualStatesAsync(snapshot, party, primaryUserId, bcProviderUpns);

        var changeLog = CompareAndLog(snapshot);
        ctx.ChangeLogs.Add(changeLog);

        if (!ctx.DryRun)
        {
            await ApplyUpdatesAsync(party, desired, plrStanding, bcProviderUpns, ctx);
        }

        return snapshot;
    }

    private static PlrStandingsDigest CalculatePlrStanding(string? cpn, Dictionary<string, IEnumerable<PlrRecord>> plrRecordsByCpn)
    {
        if (cpn != null && plrRecordsByCpn.TryGetValue(cpn, out var records))
            return PlrStandingsDigest.FromRecords(records);
        return PlrStandingsDigest.FromEmpty();
    }

    private static (string, PlrStandingsDigest) CalculateEndorsements(Party party, Dictionary<int, List<Party>> endorsementDictionary, Dictionary<string, IEnumerable<PlrRecord>> plrRecordsByCpn)
    {
        var endorsementRelations = endorsementDictionary[party.Id];
        var endorsementSummaries = new List<string>();
        var endorsementRecords = new List<PlrRecord>();
        
        foreach (var relation in endorsementRelations)
        {
            if (relation.Cpn != null && plrRecordsByCpn.TryGetValue(relation.Cpn, out var r))
            {
                endorsementRecords.AddRange(r);
                var fullStanding = PlrStandingsDigest.FromRecords(r).With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes);
                
                var isPracticing = fullStanding.HasGoodStanding ? "Practicing" : "Not Practicing";
                var identifierType = r.FirstOrDefault()?.IdentifierType;
                var collegeId = r.FirstOrDefault()?.CollegeId;
                
                endorsementSummaries.Add($"Endorsement: {relation.Id} {relation.Cpn} {identifierType} {collegeId} ({isPracticing})");
            }
        }
        
        var endorsementSummaryStr = string.Join(", ", endorsementSummaries);
        var endorsementPlrStanding = endorsementRecords.Count > 0 ? PlrStandingsDigest.FromRecords(endorsementRecords) : PlrStandingsDigest.FromEmpty();

        return (endorsementSummaryStr, endorsementPlrStanding);
    }

    private static DesiredState CalculateDesiredState(Party party, PlrStandingsDigest plrStanding, PlrStandingsDigest endorsementPlrStanding)
    {
        return new DesiredState
        {
            IsMd = plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding,
            IsMoa = !plrStanding.HasGoodStanding && endorsementPlrStanding.HasGoodStanding,
            IsPharm = plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding,
            IsRnp = plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding,
            MspIds = plrStanding.MspIds ?? Array.Empty<string>(),
            ProviderRoleTypes = plrStanding.ProviderRoleTypes.Select(x => x.ToString()).ToList(),
            CollegeIds = plrStanding.CollegeIds ?? Array.Empty<string>(),
            EndorserData = endorsementPlrStanding.WithGoodStanding().With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes).Cpns ?? Array.Empty<string>(),
            OpId = party.OpId,
            HasMdRole = plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding,
            HasMoaRole = !plrStanding.HasGoodStanding && endorsementPlrStanding.HasGoodStanding,
            HasPharmRole = plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding,
            HasRnpRole = plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding,
            HasSaRole = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.SAEforms),
            HasImmsRole = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.ImmsBCEforms),
            HasInfantRole = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.InfantRsvEforms),
            HasNpdpRole = party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.NpdpEforms)
        };
    }

    private static List<string> GetBCProviderUpns(Party party)
    {
        var upns = party.Credentials
            .Where(c => c.IdentityProvider == IdentityProviders.BCProvider)
            .Select(c => c.IdpId)
            .Where(upn => upn != null)
            .Select(upn => upn!)
            .ToList();

        if (upns.Count > 1)
        {
            throw new InvalidOperationException($"Party has multiple BCProvider credentials ({upns.Count}). Expected 0 or 1.");
        }

        return upns;
    }

    private async Task FetchActualStatesAsync(PartySyncSnapshot snapshot, Party party, Guid primaryUserId, List<string> bcProviderUpns)
    {
        if (bcProviderUpns.Count > 0)
        {
            try
            {
                var upn = bcProviderUpns.Single();
                var bcpAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId)
                    .SetIsMoa(false).SetIsMd(false).SetIsPharm(false).SetIsRnp(false)
                    .SetMspId(["unset"]).SetPractitionerRole(["unset"]).SetCollegeId(["unset"]).SetEndorserData(["unset"]);
                
                var attributes = await this.bcProviderClient.GetUserAttributes(upn, bcpAttributes.AsAdditionalData().Keys.ToArray());
                snapshot.BCProvider = new ActualBCProviderState { Upn = upn, Attributes = attributes };
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, "Party {PartyId} has multiple BCProvider credentials! Cannot fetch a single BCProvider actual state.", party.Id);
            }
        }

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
    }

    private async Task ApplyUpdatesAsync(Party party, DesiredState desired, PlrStandingsDigest plrStanding, List<string> bcProviderUpns, GlobalSyncContext ctx)
    {
        if (bcProviderUpns.Count > 0)
        {
            var bcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId);
            bcProviderAttributes.SetIsMoa(desired.IsMoa);
            bcProviderAttributes.SetIsMd(desired.IsMd);
            bcProviderAttributes.SetIsRnp(desired.IsRnp);
            bcProviderAttributes.SetIsPharm(desired.IsPharm);
            bcProviderAttributes.SetMspId(plrStanding.MspIds);
            bcProviderAttributes.SetPractitionerRole(plrStanding.ProviderRoleTypes);
            bcProviderAttributes.SetCollegeId(plrStanding.CollegeIds);
            bcProviderAttributes.SetEndorserData(desired.EndorserData);

            var additionalData = bcProviderAttributes.AsAdditionalData();
            foreach (var upn in bcProviderUpns.Where(u => !string.IsNullOrWhiteSpace(u)))
            {
                await this.SyncSingleBCProviderUpnAsync(upn, additionalData, ctx.DryRun);
            }
        }

        foreach (var userId in party.Credentials.Select(c => c.UserId))
        {
            var kctx = new KeycloakSyncContext
            {
                UserId = userId,
                Party = party,
                KeysToRemove = new[] { "college_license_info", "college_licence_info", "college_certification_info" },
                PlrStanding = plrStanding,
                IsMd = desired.IsMd,
                IsMoa = desired.IsMoa,
                IsPharm = desired.IsPharm,
                IsRnp = desired.IsRnp,
                DryRun = ctx.DryRun,
                LicenceStatusClientId = "LICENCE-STATUS",
                EformsClientId = "SAT-EFORMS",
                SaRole = ctx.KeycloakRoles.SaRole,
                ImmsRole = ctx.KeycloakRoles.ImmsRole,
                InfantRole = ctx.KeycloakRoles.InfantRole,
                NpdpRole = ctx.KeycloakRoles.NpdpRole,
                GlobalContext = ctx
            };
            await this.SyncKeycloakUserAsync(kctx);
        }
    }

    private async Task WriteJsonOutputAsync(List<PartySyncSnapshot> snapshots)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var filename = $"output/resync_{timestamp}.json";
        var jsonContent = JsonSerializer.Serialize(snapshots, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filename, jsonContent);
        Console.WriteLine($"Wrote JSON output to {filename}");
    }

    private async Task WriteExceptionsOutputAsync(List<string> exceptions)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var filename = $"output/exceptions_{timestamp}.log";
        await File.WriteAllLinesAsync(filename, exceptions);
        Console.WriteLine($"Wrote Exceptions output to {filename}");
    }

    private async Task WriteEformsDriftOutputAsync(List<EformsDrift> drifts)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var filename = $"output/eforms-drift_{timestamp}.json";
        var jsonContent = JsonSerializer.Serialize(drifts, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        await File.WriteAllTextAsync(filename, jsonContent);
        Console.WriteLine($"Wrote Eforms Drift output to {filename}");
    }

    private async Task WriteChangesOutputAsync(List<PartyChangeLog> logs)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var filename = $"output/changes_{timestamp}.json";
        var jsonContent = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        await File.WriteAllTextAsync(filename, jsonContent);
        Console.WriteLine($"Wrote Changes output to {filename}");
    }

    // Refactored CompareAndLog helpers
    private static PartyChangeLog CompareAndLog(PartySyncSnapshot snapshot)
    {
        var header = LogPartyDetails(snapshot);

        var bcChanges = new List<string>();
        var kcChanges = new List<string>();

        CompareBCProvider(snapshot, bcChanges);
        CompareKeycloak(snapshot, kcChanges);
        
        if (bcChanges.Count > 0) Console.WriteLine($"    BCProvider Changes: {string.Join(", ", bcChanges)}");
        if (kcChanges.Count > 0) Console.WriteLine($"    Keycloak Changes: {string.Join(", ", kcChanges)}");
        if (bcChanges.Count == 0 && kcChanges.Count == 0) Console.WriteLine("    No changes required.");

        return new PartyChangeLog
        {
            PartyHeader = header,
            EndorserData = snapshot.Expected.EndorserData.Any() ? string.Join(",", snapshot.Expected.EndorserData) : null,
            BCProviderChanges = bcChanges.Count > 0 ? bcChanges : null,
            KeycloakChanges = kcChanges.Count > 0 ? kcChanges : null
        };
    }

    private static string LogPartyDetails(PartySyncSnapshot snapshot)
    {
        var expected = snapshot.Expected;
        var licenses = expected.CollegeIds.Any() ? string.Join(",", expected.CollegeIds) : "None";
        var roles = expected.ProviderRoleTypes.Any() ? string.Join(",", expected.ProviderRoleTypes) : "None";

        var header = $"Party: {snapshot.PartyId} {snapshot.FirstName} {snapshot.LastName} {snapshot.Cpn} {licenses} {roles}".Trim();
        Console.WriteLine();
        Console.WriteLine(header);
        if (!string.IsNullOrEmpty(snapshot.EndorsementSummary))
        {
            Console.WriteLine($"    {snapshot.EndorsementSummary}");
        }

        foreach (var cred in snapshot.Credentials)
        {
            var providerStr = cred.IdentityProvider switch
            {
                IdentityProviders.BCProvider => "bcp",
                IdentityProviders.BCServicesCard => "bcsc",
                _ => cred.IdentityProvider
            };

            var attrsJson = "";
            if (cred.IdentityProvider == IdentityProviders.BCProvider && snapshot.BCProvider?.Attributes != null)
            {
                attrsJson = ", Attributes: " + JsonSerializer.Serialize(snapshot.BCProvider.Attributes);
            }
            Console.WriteLine($"    {providerStr}: {cred.IdpId}{attrsJson}");
        }

        if (snapshot.Keycloak?.Attributes != null)
        {
            Console.WriteLine($"    keycloak: {snapshot.UserId}, Attributes: {JsonSerializer.Serialize(snapshot.Keycloak.Attributes)}");
        }

        return header;
    }

    private static void CompareBCProvider(PartySyncSnapshot snapshot, List<string> bcChanges)
    {
        if (snapshot.BCProvider?.Attributes == null) return;
        
        var attrs = snapshot.BCProvider.Attributes;
        string GetBcpValue(string suffix) 
        {
            var key = attrs.Keys.FirstOrDefault(k => k.EndsWith(suffix, StringComparison.OrdinalIgnoreCase));
            return (key != null && attrs.TryGetValue(key, out var val)) ? (val?.ToString()?.ToLower() ?? "null") : "null";
        }

        void CheckBc(string prop, string expectedStr, string actualStr)
        {
            if (prop == "EndorserData" && expectedStr == "null" && actualStr == "[]")
            {
                return;
            }

            if (prop == "isPharm" && expectedStr == "false" && actualStr == "null")
            {
                return;
            }

            if (expectedStr != actualStr)
            {
                var displayActual = (string.IsNullOrEmpty(actualStr) || actualStr == "null") ? "Unset" : actualStr;
                bcChanges.Add($"{prop}: {displayActual} -> {expectedStr}");
            }
        }

        CheckBc("isMoa", snapshot.Expected.IsMoa.ToString().ToLower(), GetBcpValue("_isMoa"));
        CheckBc("isMd", snapshot.Expected.IsMd.ToString().ToLower(), GetBcpValue("_isMd"));
        CheckBc("isPharm", snapshot.Expected.IsPharm.ToString().ToLower(), GetBcpValue("_isPharm"));
        CheckBc("isRnp", snapshot.Expected.IsRnp.ToString().ToLower(), GetBcpValue("_isRnp"));

        string ArrayToStr(IEnumerable<string> arr)
        {
            var str = ("[" + string.Join(",", arr.Select(s => $"\"{s}\"")) + "]").ToLower();
            return str == "[]" ? "null" : str;
        }

        CheckBc("CollegeId", ArrayToStr(snapshot.Expected.CollegeIds), GetBcpValue("_collegeid"));
        CheckBc("MspId", ArrayToStr(snapshot.Expected.MspIds), GetBcpValue("_mspId"));
        CheckBc("PractitionerRole", ArrayToStr(snapshot.Expected.ProviderRoleTypes), GetBcpValue("_practitionerRole"));
        CheckBc("EndorserData", ArrayToStr(snapshot.Expected.EndorserData), GetBcpValue("_endorserData"));
    }

    private static void CompareKeycloak(PartySyncSnapshot snapshot, List<string> kcChanges)
    {
        if (snapshot.Keycloak == null) return;

        var attrs = snapshot.Keycloak.Attributes;
        string GetKcValue(string key) => attrs.GetValueOrDefault(key)?.FirstOrDefault()?.ToLower() ?? "null";

        var obsoleteKeys = new[] { "college_license_info", "college_licence_info", "college_certification_info" };
        foreach (var key in obsoleteKeys)
        {
            if (attrs.ContainsKey(key))
            {
                kcChanges.Add($"Remove obsolete key: {key}");
            }
        }
    }

    private async Task SyncSingleBCProviderUpnAsync(string upn, Dictionary<string, object> additionalData, bool dryRun)
    {
        var currentAttributes = await this.bcProviderClient.GetUserAttributes(upn, additionalData.Keys.ToArray());
        var hasChanges = currentAttributes == null;
        var keysToRemove = new List<string>();

        if (hasChanges)
        {
            this.logger.LogWarning("UPN {Upn} Could not retrieve current attributes from Entra.", upn);
        }
        else
        {
            var keysToCheck = currentAttributes!.Keys.Union(additionalData.Keys).ToList();

            foreach (var key in keysToCheck)
            {
                additionalData.TryGetValue(key, out var newVal);
                var newValueString = newVal?.ToString()?.ToLowerInvariant() ?? "null";
                
                var isEndorserData = key.EndsWith("_endorserData", StringComparison.OrdinalIgnoreCase);
                var isIsPharm = key.EndsWith("_isPharm", StringComparison.OrdinalIgnoreCase);
                
                var currentValueString = currentAttributes!.TryGetValue(key, out var currVal) ? (currVal?.ToString()?.ToLowerInvariant() ?? "null") : "null";

                if (isIsPharm && newValueString == "false" && currentValueString == "null")
                {
                    newValueString = "null";
                    newVal = null;
                }

                if (newValueString == "[]")
                {
                    if (isEndorserData && currentValueString == "[]")
                    {
                        // Leave newValueString as "[]", so it matches currentValueString and prevents hasChanges = true
                    }
                    else
                    {
                        newValueString = "null";
                        newVal = null;
                    }
                }

                if (newValueString != currentValueString)
                {
                    this.logger.LogInformation("UPN {Upn} Attribute {Key} changing from {CurrentValueString} to {NewValueString}", upn, key, currentValueString, newValueString);
                    hasChanges = true;
                    additionalData[key] = newVal!;
                }
                else if (newValueString == "null")
                {
                    keysToRemove.Add(key);
                }
            }
        }

        foreach (var key in keysToRemove)
        {
            additionalData.Remove(key);
        }

        if (hasChanges && additionalData.Count > 0 && !dryRun)
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

            var hasSaEforms = ctx.Party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.SAEforms);
            var hasImms = ctx.Party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.ImmsBCEforms);
            var hasInfant = ctx.Party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.InfantRsvEforms);
            var hasNpdp = ctx.Party.AccessRequests.Any(ar => ar.AccessTypeCode == AccessTypeCode.NpdpEforms);

            var eformsClientInternalId = ctx.SaRole?.ContainerId ?? ctx.ImmsRole?.ContainerId ?? ctx.InfantRole?.ContainerId ?? ctx.NpdpRole?.ContainerId;
            if (eformsClientInternalId != null)
            {
                var currentUserRoles = await this.keycloakClient.GetUserClientRoles(ctx.UserId, eformsClientInternalId);
                var currentRoleNames = currentUserRoles?.Select(r => r.Name).ToHashSet() ?? new HashSet<string>();

                var addList = new List<string>();
                var removeList = new List<string>();

                void CheckRole(string roleName, bool shouldHave)
                {
                    var hasRole = currentRoleNames.Contains(roleName);
                    if (shouldHave && !hasRole) addList.Add(roleName);
                    if (!shouldHave && hasRole) removeList.Add(roleName);
                }

                CheckRole("phsa_eforms_sat", hasSaEforms);
                CheckRole("phsa_eforms_imms", hasImms);
                CheckRole("phsa_eforms_infant_rsv", hasInfant);
                CheckRole("phsa_eforms_npdp", hasNpdp);

                if (addList.Count > 0 || removeList.Count > 0)
                {
                    ctx.GlobalContext.EformsDrifts.Add(new EformsDrift
                    {
                        PartyId = ctx.Party.Id,
                        Add = addList.Count > 0 ? string.Join(",", addList) : null,
                        Remove = removeList.Count > 0 ? string.Join(",", removeList) : null
                    });
                }
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
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? SaRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? ImmsRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? InfantRole { get; set; }
        public Pidp.Infrastructure.HttpClients.Keycloak.Role? NpdpRole { get; set; }
        public GlobalSyncContext GlobalContext { get; set; } = null!;
    }

    private static bool SetKeycloakAttribute(Pidp.Infrastructure.HttpClients.Keycloak.UserRepresentation user, string key, IEnumerable<string> EnumerableNewValue)
    {
        var newValueList = EnumerableNewValue.ToList();
        if (newValueList.Count == 1 && newValueList[0] == "[]")
        {
            newValueList.Clear();
        }

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
        var allPassed = true;

        try
        {
            var canConnect = await this.context.Database.CanConnectAsync();
            if (!canConnect) Console.WriteLine("Database: FAIL");
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
            if (plrTest == null) Console.WriteLine("PLR Webservice: FAIL");
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
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Keycloak: FAIL ({ex.Message})");
            allPassed = false;
        }

        try
        {
            await this.bcProviderClient.GetUserAttributes("test-connection@example.com", Array.Empty<string>());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"BCProvider: FAIL ({ex.Message})");
            allPassed = false;
        }

        return allPassed;
    }
}
