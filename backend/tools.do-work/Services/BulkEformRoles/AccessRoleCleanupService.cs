namespace DoWork.Services.BulkEformRoles;

using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using Pidp;
using Pidp.Data;
using Pidp.Extensions;
using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models.Lookups;

/// <summary>
/// Re-evaluates every holder of one Access Type and revokes those who no longer qualify.
///
/// Owns no eligibility logic: it runs the same IAccessRequestRevocationPolicy the webapi runs on every PLR
/// distribution, so a bulk sweep cannot disagree with live behaviour. Any card that gains a policy becomes
/// available here with no change to this file.
/// </summary>
public class AccessRoleCleanupService(
    BulkRunOptions options,
    IEnumerable<IAccessRequestRevocationPolicy> policies,
    IAccessRequestRevocationService revocationService,
    IHostApplicationLifetime lifetime,
    IPlrClient plrClient,
    PidpDbContext context,
    PidpConfiguration config) : IAccessRoleCleanupService
{
    public const string ReportKind = "access-role-cleanup";

    /// <summary>The outcome a dry run records for a Party it proposes to revoke, and the only one an apply run acts on.</summary>
    public const string WouldRevokeOutcome = "WouldRevoke";

    private readonly BulkRunOptions options = options;
    private readonly IEnumerable<IAccessRequestRevocationPolicy> policies = policies;
    private readonly IAccessRequestRevocationService revocationService = revocationService;
    private readonly IHostApplicationLifetime lifetime = lifetime;
    private readonly IPlrClient plrClient = plrClient;
    private readonly PidpDbContext context = context;
    private readonly PidpConfiguration config = config;

    public async Task CleanupAsync()
    {
        Console.WriteLine(this.options.ModeBanner);
        Console.WriteLine();

        // Proves the dry-run seam is actually in place. Without it there is no way to tell a run that
        // wrote nothing from a run that wrote everything.
        if (this.revocationService is not RecordingRevocationService)
        {
            Console.WriteLine("ERROR: expected a RecordingRevocationService. Refusing to run rather than act blind.");
            return;
        }

        var supported = this.policies.OrderBy(policy => policy.AccessTypeCode.ToString(), StringComparer.Ordinal).ToList();

        var policy = this.ResolveAccessType(supported);
        if (policy == null)
        {
            return;
        }

        var accessTypeCode = policy.AccessTypeCode;

        HashSet<int>? approvedPartyIds = null;
        if (this.options.FromCsv != null)
        {
            // An apply run acts only on the rows that proposed a revocation. A dry run re-reads the whole
            // file, because narrowing a dry run is a way of re-examining a population, not of approving it.
            var filter = new ReportFilter(
                ReportKind,
                accessTypeCode.ToString(),
                this.options.Apply ? WouldRevokeOutcome : null);

            if (!ReviewedReport.TryRead(this.options.FromCsv, filter, out approvedPartyIds, out var summary, out var error))
            {
                Console.WriteLine($"ERROR: {error}");
                return;
            }

            Console.WriteLine(summary);
        }

        // An unpopulated table would look exactly like nobody being in good standing, and revoke everyone.
        if (this.plrClient is PlrRecordDbClient db && !await db.AnyRecordsExistAsync())
        {
            Console.WriteLine("ERROR: the Plr_PlrRecord table is empty. Aborting.");
            return;
        }

        var holders = await this.GetHoldersAsync(accessTypeCode, approvedPartyIds);
        if (holders.Count == 0)
        {
            Console.WriteLine($"No Parties in scope for {accessTypeCode}. Nothing to do.");
            return;
        }

        Console.WriteLine($"{holders.Count} Parties in scope for {accessTypeCode}.");

        if (this.options.Apply)
        {
            Console.WriteLine();
            RunTarget.Describe(this.config);

            if (!RunTarget.Confirm(accessTypeCode.ToString()))
            {
                return;
            }
        }

        await this.RunAsync(policy, accessTypeCode, holders);
    }

    private IAccessRequestRevocationPolicy? ResolveAccessType(List<IAccessRequestRevocationPolicy> supported)
    {
        if (supported.Count == 0)
        {
            Console.WriteLine("ERROR: no revocation policies are registered.");
            return null;
        }

        if (this.options.AccessType != null)
        {
            var match = supported.SingleOrDefault(policy => policy.AccessTypeCode.ToString().Equals(this.options.AccessType, StringComparison.OrdinalIgnoreCase));
            if (match == null)
            {
                Console.WriteLine($"ERROR: '{this.options.AccessType}' has no revocation policy. Supported: {string.Join(", ", supported.Select(policy => policy.AccessTypeCode))}");
            }

            return match;
        }

        Console.WriteLine("Select the Access Type to evaluate:");
        for (var i = 0; i < supported.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {supported[i].AccessTypeCode}");
        }
        Console.Write($"Enter your choice (1-{supported.Count}): ");

        if (int.TryParse(Console.ReadLine()?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var choice)
            && choice >= 1
            && choice <= supported.Count)
        {
            return supported[choice - 1];
        }

        Console.WriteLine("Invalid choice. Aborting.");
        return null;
    }

    private async Task RunAsync(IAccessRequestRevocationPolicy policy, AccessTypeCode accessTypeCode, List<Holder> holders)
    {
        var apply = this.options.Apply;
        var stopping = this.lifetime.ApplicationStopping;

        using var report = new CsvReport(
            ReportKind,
            CsvReport.PartyIdColumn, "FirstName", "LastName", "Cpn", "IdentifierType", "ProviderRoleType",
            CsvReport.AccessTypeColumn, CsvReport.OutcomeColumn, "Reason", "Detail");

        var tally = new Dictionary<string, int>(StringComparer.Ordinal);
        var stopped = false;

        for (var i = 0; i < holders.Count; i++)
        {
            // Checked between Parties, never during one. A Party is a Keycloak removal followed by a
            // database delete, and cancelling between those two leaves a User with no access and a record
            // saying they still have it. Finishing the Party in flight costs one extra iteration.
            if (stopping.IsCancellationRequested)
            {
                Console.WriteLine($"Cancellation requested. Stopped after {i} of {holders.Count}; everything already applied has been saved.");
                stopped = true;
                break;
            }

            var holder = holders[i];

            if (i > 0 && i % 100 == 0)
            {
                Console.WriteLine($"[{i}/{holders.Count}] processed...");
            }

            var decision = await policy.RevokeIfIneligibleAsync(holder.PartyId);

            // Saved per Party rather than in batches. The Keycloak call has already happened by the time
            // the delete is staged, so anything still unsaved when the process dies is a User whose role
            // is gone but whose Access Request - and whose audit event - says otherwise. Against three
            // Keycloak round trips per Party, one extra commit costs nothing.
            if (apply && decision.WasIneligible)
            {
                await this.context.SaveChangesAsync(CancellationToken.None);
            }

            var outcome = OutcomeFor(decision, apply);
            var records = (await this.plrClient.GetRecordsAsync(holder.Cpn) ?? []).ToList();

            tally[outcome] = tally.GetValueOrDefault(outcome) + 1;
            report.WriteRow(
                holder.PartyId,
                holder.FirstName,
                holder.LastName,
                holder.Cpn,
                Distinct(records, record => record.IdentifierType),
                Distinct(records, record => record.ProviderRoleType),
                accessTypeCode,
                outcome,
                decision.Reason,
                decision.WasIneligible ? string.Empty : await this.DiagnoseAsync(holder, records));
        }

        if (apply && this.context.ChangeTracker.HasChanges())
        {
            await this.context.SaveChangesAsync(CancellationToken.None);
        }

        if (!apply && this.context.ChangeTracker.HasChanges())
        {
            Console.WriteLine("ERROR: a dry run staged database changes. Discarding them; do not trust this report.");
            this.context.ChangeTracker.Clear();
        }

        var mode = apply ? "APPLIED" : "DRY RUN - nothing written";
        Console.WriteLine(Environment.NewLine + $"--- {accessTypeCode} Role Cleanup Complete ({mode}{(stopped ? ", STOPPED EARLY" : string.Empty)}) ---");
        foreach (var entry in tally.OrderBy(entry => entry.Key, StringComparer.Ordinal))
        {
            Console.WriteLine($"  {entry.Key}: {entry.Value}");
        }
        Console.WriteLine($"Report written to {report.FilePath}");

        if (!apply)
        {
            Console.WriteLine();
            Console.WriteLine($"Review the report. Only rows with {CsvReport.OutcomeColumn} = {WouldRevokeOutcome} will be acted on;");
            Console.WriteLine("delete any of those you want to exclude, then re-run with:");
            Console.WriteLine($"  dotnet run -- --access-type {accessTypeCode} --apply --from '{report.FilePath}'");
        }
    }

    private static string OutcomeFor(RevocationDecision decision, bool apply) => decision.Outcome switch
    {
        RevocationOutcome.NotHeld => "NotHeld",
        RevocationOutcome.StandingUnknown => "StandingUnknown",
        RevocationOutcome.Eligible => "NoAction",
        RevocationOutcome.Revoked => apply ? "Revoked" : WouldRevokeOutcome,
        RevocationOutcome.RevokeFailed => "RevokeFailed",
        _ => decision.Outcome.ToString()
    };

    /// <summary>
    /// Extra PLR context for a Party the policy left alone, so a gap in PLR data does not look identical
    /// to everyone being eligible.
    /// </summary>
    /// <param name="holder"></param>
    /// <param name="records"></param>
    private async Task<string> DiagnoseAsync(Holder holder, List<PlrRecord> records)
    {
        if (holder.Cpn == null)
        {
            var endorsements = await this.plrClient.GetAggregateStandingsDigestAsync(holder.EndorsementCpns);

            return holder.EndorsementCpns.Count == 0
                ? "no CPN and no active endorsements"
                : $"no CPN; {holder.EndorsementCpns.Count} endorsement CPN(s), endorsers in good standing: {endorsements.HasGoodStanding}";
        }

        if (records.Count == 0)
        {
            return "NO PLR RECORD for this CPN - standing unknown, left in place";
        }

        var standing = PlrStandingsDigest.FromRecords(records);

        return $"PLR record found; good standing: {standing.HasGoodStanding}, CPS postgrad: {standing.IsCpsPostgrad}";
    }

    private static string Distinct(List<PlrRecord> records, Func<PlrRecord, string?> select)
    {
        return string.Join('|', records
            .Select(select)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.Ordinal));
    }

    private async Task<List<Holder>> GetHoldersAsync(AccessTypeCode accessTypeCode, HashSet<int>? approvedPartyIds)
    {
        Console.WriteLine($"Querying database for {accessTypeCode} holders...");

        var holders = await this.context.Parties
            .Where(party => party.AccessRequests.Any(request => request.AccessTypeCode == accessTypeCode)
                && (approvedPartyIds == null || approvedPartyIds.Contains(party.Id)))
            .OrderBy(party => party.Id)
            .Select(party => new Holder
            {
                PartyId = party.Id,
                FirstName = party.DisplayFirstName,
                LastName = party.DisplayLastName,
                Cpn = party.Cpn
            })
            .ToListAsync();

        foreach (var holder in holders.Where(holder => holder.Cpn == null))
        {
            holder.EndorsementCpns = await this.context.ActiveEndorsementRelationships(holder.PartyId)
                .Select(relationship => relationship.Party!.Cpn)
                .ToListAsync();
        }

        return holders;
    }

    private sealed class Holder
    {
        public int PartyId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Cpn { get; set; }
        public List<string?> EndorsementCpns { get; set; } = [];
    }
}
