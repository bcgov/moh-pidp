namespace DoWork.Services.BulkEformRoles;

/// <summary>
/// Reads back a report from a previous run and works out which Parties this run is allowed to touch.
///
/// Every check here fails closed. A report whose columns were dropped, renamed or mangled - by a reviewer
/// editing it in Excel, or by pointing the tool at the wrong file - is rejected rather than treated as
/// permission to act on everything in it.
/// </summary>
/// <param name="Kind">The tool the report must have come from. A report is never valid for another tool.</param>
/// <param name="AccessType">The card the report must cover, for tools that evaluate one card at a time.</param>
/// <param name="RequiredOutcome">
/// When set, only rows recording this outcome are acted on. This is what makes the review meaningful: a dry
/// run reports a row for every Party it looked at, so approving the file must not be read as approving an
/// action against every Party in it - only against the ones actually proposed for a change.
/// </param>
public sealed record ReportFilter(string Kind, string? AccessType = null, string? RequiredOutcome = null);

public static class ReviewedReport
{
    /// <param name="path"></param>
    /// <param name="filter"></param>
    /// <param name="partyIds">The Parties this run may act on.</param>
    /// <param name="summary">What the file contained and what was dropped, for the operator to read.</param>
    /// <param name="error"></param>
    public static bool TryRead(
        string path,
        ReportFilter filter,
        out HashSet<int> partyIds,
        out string summary,
        out string? error)
    {
        partyIds = [];
        summary = string.Empty;
        error = null;

        var fileName = Path.GetFileName(path);
        var rows = CsvReport.Read(path);

        if (rows.Count == 0)
        {
            error = $"{fileName} contains no rows.";
            return false;
        }

        var kinds = CsvReport.DistinctValues(rows, CsvReport.KindColumn);
        if (kinds.Count != 1 || !kinds.Contains(filter.Kind))
        {
            error = kinds.Count == 0
                ? $"{fileName} has no '{CsvReport.KindColumn}' column. Only a report produced by a dry run of this tool can be passed to --from."
                : $"{fileName} is a '{string.Join("', '", kinds)}' report, but this run is '{filter.Kind}'.";
            return false;
        }

        if (filter.AccessType != null)
        {
            var accessTypes = CsvReport.DistinctValues(rows, CsvReport.AccessTypeColumn);
            if (accessTypes.Count != 1 || !accessTypes.Contains(filter.AccessType))
            {
                error = accessTypes.Count == 0
                    ? $"{fileName} has no '{CsvReport.AccessTypeColumn}' column, so it cannot be confirmed to cover {filter.AccessType}."
                    : $"{fileName} covers {string.Join(", ", accessTypes)}, but this run is for {filter.AccessType}.";
                return false;
            }
        }

        var actionable = rows;
        if (filter.RequiredOutcome != null)
        {
            if (CsvReport.DistinctValues(rows, CsvReport.OutcomeColumn).Count == 0)
            {
                error = $"{fileName} has no '{CsvReport.OutcomeColumn}' column, so the rows proposing a change cannot be identified.";
                return false;
            }

            actionable = rows
                .Where(row => CsvReport.HasValue(row, CsvReport.OutcomeColumn, filter.RequiredOutcome))
                .ToList();

            if (actionable.Count == 0)
            {
                error = $"{fileName} has no rows with {CsvReport.OutcomeColumn} = {filter.RequiredOutcome}. There is nothing to apply.";
                return false;
            }
        }

        partyIds = CsvReport.PartyIds(actionable);

        if (partyIds.Count == 0)
        {
            error = $"{fileName} has no readable '{CsvReport.PartyIdColumn}' values.";
            return false;
        }

        summary = filter.RequiredOutcome == null
            ? $"Limited by {fileName}: {partyIds.Count} Parties."
            : $"Limited by {fileName}: {partyIds.Count} Parties with {CsvReport.OutcomeColumn} = {filter.RequiredOutcome} "
                + $"({rows.Count - actionable.Count} of {rows.Count} rows proposed no change and are ignored).";

        return true;
    }
}
