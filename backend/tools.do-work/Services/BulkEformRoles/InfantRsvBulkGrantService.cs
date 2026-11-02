namespace DoWork.Services.BulkEformRoles;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using Pidp;
using Pidp.Data;
using Pidp.Extensions;
using Pidp.Features;
using Pidp.Features.AccessRequests;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

using DomainResults.Common;

/// <summary>
/// Grants the Infant RSV eForm role to existing Imms / SA eForm users who qualify for it.
///
/// Self-contained: it does not depend on the Access Role Cleanup having been run first, and does not trust
/// the standing implied by an existing Imms or SA holding. Those two cards define the population - who this
/// back-grant is for - but every candidate's RSV eligibility is then evaluated from PLR directly, by the
/// same InfantRsvEforms rules a self-serve enrolment uses. A Party still holding Imms after their licence
/// lapsed is therefore reported SkippedIneligible here, whether or not anyone has cleaned up their Imms.
///
/// Applying delegates to InfantRsvEforms.CommandHandler, the same handler that serves a self-serve
/// enrolment, so a bulk grant and a portal request are the same operation.
/// </summary>
public class InfantRsvBulkGrantService(
    BulkRunOptions options,
    ICommandHandler<InfantRsvEforms.Command, IDomainResult> grantHandler,
    IHostApplicationLifetime lifetime,
    IPlrClient plrClient,
    PidpDbContext context,
    PidpConfiguration config) : IInfantRsvBulkGrantService
{
    public const string ReportKind = "infant-rsv-bulk-grant";

    /// <summary>The outcome a dry run records for a Party it proposes to grant, and the only one an apply run acts on.</summary>
    public const string WouldGrantOutcome = "WouldGrant";

    private readonly BulkRunOptions options = options;
    private readonly ICommandHandler<InfantRsvEforms.Command, IDomainResult> grantHandler = grantHandler;
    private readonly IHostApplicationLifetime lifetime = lifetime;
    private readonly IPlrClient plrClient = plrClient;
    private readonly PidpDbContext context = context;
    private readonly PidpConfiguration config = config;

    public async Task GrantAsync()
    {
        var apply = this.options.Apply;
        var stopping = this.lifetime.ApplicationStopping;

        Console.WriteLine(this.options.ModeBanner);
        Console.WriteLine();

        HashSet<int>? approvedPartyIds = null;
        if (this.options.FromCsv != null)
        {
            var filter = new ReportFilter(
                ReportKind,
                AccessTypeCode.InfantRsvEforms.ToString(),
                apply ? WouldGrantOutcome : null);

            if (!ReviewedReport.TryRead(this.options.FromCsv, filter, out approvedPartyIds, out var summary, out var error))
            {
                Console.WriteLine($"ERROR: {error}");
                return;
            }

            Console.WriteLine(summary);
        }

        if (this.plrClient is PlrRecordDbClient db && !await db.AnyRecordsExistAsync())
        {
            Console.WriteLine("ERROR: the Plr_PlrRecord table is empty. Aborting.");
            return;
        }

        var candidates = await this.GetCandidatesAsync(approvedPartyIds);
        if (candidates.Count == 0)
        {
            Console.WriteLine("No candidates found. Nothing to do.");
            return;
        }

        Console.WriteLine($"{candidates.Count} candidate Parties hold Imms and/or SA eForms without Infant RSV.");

        if (apply)
        {
            Console.WriteLine();
            RunTarget.Describe(this.config);

            if (!RunTarget.Confirm(AccessTypeCode.InfantRsvEforms.ToString()))
            {
                return;
            }
        }

        using var report = new CsvReport(
            ReportKind,
            CsvReport.PartyIdColumn, "FirstName", "LastName", "Cpn", "IdentifierType", "ProviderRoleType",
            CsvReport.AccessTypeColumn, "Basis", "HeldRoles", CsvReport.OutcomeColumn);

        var tally = new Dictionary<string, int>(StringComparer.Ordinal);
        var stopped = false;

        for (var i = 0; i < candidates.Count; i++)
        {
            // Checked between Parties, never during one, so a Party is never left half-granted.
            if (stopping.IsCancellationRequested)
            {
                Console.WriteLine($"Cancellation requested. Stopped after {i} of {candidates.Count}; everything already applied has been saved.");
                stopped = true;
                break;
            }

            var candidate = candidates[i];

            if (i > 0 && i % 100 == 0)
            {
                Console.WriteLine($"[{i}/{candidates.Count}] processed...");
            }

            var outcome = await this.ProcessAsync(candidate, apply);
            var records = (await this.plrClient.GetRecordsAsync(candidate.Cpn) ?? []).ToList();

            tally[outcome] = tally.GetValueOrDefault(outcome) + 1;
            report.WriteRow(
                candidate.PartyId,
                candidate.FirstName,
                candidate.LastName,
                candidate.Cpn,
                Distinct(records, record => record.IdentifierType),
                Distinct(records, record => record.ProviderRoleType),
                AccessTypeCode.InfantRsvEforms,
                candidate.Cpn == null ? "Endorsement" : "OwnStanding",
                string.Join('|', candidate.HeldAccessTypes),
                outcome);
        }

        var mode = apply ? "APPLIED" : "DRY RUN - nothing written";
        Console.WriteLine(Environment.NewLine + $"--- Infant RSV Bulk Grant Complete ({mode}{(stopped ? ", STOPPED EARLY" : string.Empty)}) ---");
        foreach (var entry in tally.OrderBy(entry => entry.Key, StringComparer.Ordinal))
        {
            Console.WriteLine($"  {entry.Key}: {entry.Value}");
        }
        Console.WriteLine($"Report written to {report.FilePath}");

        if (!apply)
        {
            Console.WriteLine();
            Console.WriteLine($"Review the report. Only rows with {CsvReport.OutcomeColumn} = {WouldGrantOutcome} will be acted on;");
            Console.WriteLine("delete any of those you want to exclude, then re-run with:");
            Console.WriteLine($"  dotnet run -- --apply --from '{report.FilePath}'");
        }
    }

    private async Task<string> ProcessAsync(Candidate candidate, bool apply)
    {
        var eligible = candidate.Cpn == null
            ? InfantRsvEforms.IsEligibleByEndorsement(await this.plrClient.GetAggregateStandingsDigestAsync(candidate.EndorsementCpns))
            : InfantRsvEforms.IsEligible(await this.plrClient.GetStandingsDigestAsync(candidate.Cpn));

        if (!eligible)
        {
            return "SkippedIneligible";
        }

        if (!apply)
        {
            return WouldGrantOutcome;
        }

        var result = await this.grantHandler.HandleAsync(new InfantRsvEforms.Command { PartyId = candidate.PartyId });

        return result.IsSuccess ? "Granted" : "GrantFailed";
    }

    private static string Distinct(List<PlrRecord> records, Func<PlrRecord, string?> select)
    {
        return string.Join('|', records
            .Select(select)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Order(StringComparer.Ordinal));
    }

    /// <summary>
    /// Mirrors the guards in InfantRsvEforms.CommandHandler. Excluding Parties who already hold the
    /// enrolment is what makes a re-run resume where the last one stopped.
    /// </summary>
    /// <param name="approvedPartyIds"></param>
    private async Task<List<Candidate>> GetCandidatesAsync(HashSet<int>? approvedPartyIds)
    {
        Console.WriteLine("Querying database for candidate Parties...");

        var candidates = await this.context.Parties
            .Where(party => party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.ImmsBCEforms
                    || request.AccessTypeCode == AccessTypeCode.SAEforms)
                && !party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.InfantRsvEforms)
                && party.Email != null
                && party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard)
                && (approvedPartyIds == null || approvedPartyIds.Contains(party.Id)))
            .OrderBy(party => party.Id)
            .Select(party => new Candidate
            {
                PartyId = party.Id,
                FirstName = party.DisplayFirstName,
                LastName = party.DisplayLastName,
                Cpn = party.Cpn,
                HeldAccessTypes = party.AccessRequests
                    .Where(request => request.AccessTypeCode == AccessTypeCode.ImmsBCEforms
                        || request.AccessTypeCode == AccessTypeCode.SAEforms)
                    .Select(request => request.AccessTypeCode)
                    .ToList()
            })
            .ToListAsync();

        foreach (var candidate in candidates.Where(candidate => candidate.Cpn == null))
        {
            candidate.EndorsementCpns = await this.context.ActiveEndorsementRelationships(candidate.PartyId)
                .Select(relationship => relationship.Party!.Cpn)
                .ToListAsync();
        }

        return candidates;
    }

    private sealed class Candidate
    {
        public int PartyId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Cpn { get; set; }
        public List<AccessTypeCode> HeldAccessTypes { get; set; } = [];
        public List<string?> EndorsementCpns { get; set; } = [];
    }
}
