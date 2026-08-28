namespace DoWork.Services.BulkEformRoles;

/// <summary>
/// How a bulk tool run was invoked. Dry run is the default and requires no arguments, so the only way
/// to change live access is to say so explicitly AND name the list that was reviewed.
/// </summary>
/// <param name="Apply">False (the default) means compute and report; write nothing.</param>
/// <param name="FromCsv">
/// A report from a previous run. An apply run is limited to the rows in it that actually proposed a change
/// - a dry run writes a row for every Party it examined, and approving the file must not be read as
/// approving an action against all of them. A dry run passed --from re-examines every row instead, since
/// narrowing a dry run approves nothing.
/// </param>
/// <param name="AccessType">
/// Which card to evaluate, e.g. "ImmsBCEforms". Kept as a string rather than AccessTypeCode so this type
/// stays free of any webapi dependency; the cleanup service resolves it against the cards that actually
/// have a revocation policy, which is the only authoritative list.
/// </param>
public sealed record BulkRunOptions(bool Apply, string? FromCsv, string? AccessType = null)
{
    public static BulkRunOptions DryRun { get; } = new(false, null);

    public string ModeBanner => this.Apply
        ? $"*** APPLY - this run WILL change live access, limited to the proposed changes in {Path.GetFileName(this.FromCsv)} ***"
        : this.FromCsv == null
            ? "*** DRY RUN - nothing will be written ***"
            : $"*** DRY RUN - nothing will be written; limited to the rows in {Path.GetFileName(this.FromCsv)} ***";

    /// <summary>
    /// Accepted forms:
    ///   (none)                            dry run over every candidate
    ///   --access-type &lt;name&gt;        which card to evaluate; prompts if omitted
    ///   --from &lt;path&gt;               dry run over only the Parties named in a previous report
    ///   --apply --from &lt;path&gt;       act on the Parties named in a previous report
    /// </summary>
    /// <param name="args"></param>
    /// <param name="options"></param>
    /// <param name="error"></param>
    public static bool TryParse(string[] args, out BulkRunOptions options, out string? error)
    {
        options = DryRun;
        error = null;

        var apply = false;
        string? fromCsv = null;
        string? accessType = null;

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg.Equals("--apply", StringComparison.OrdinalIgnoreCase))
            {
                apply = true;
            }
            else if (arg.StartsWith("--from=", StringComparison.OrdinalIgnoreCase))
            {
                fromCsv = arg["--from=".Length..].Trim('"');
            }
            else if (arg.Equals("--from", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 >= args.Length)
                {
                    error = "--from must be followed by the path to a report from a previous run.";
                    return false;
                }

                fromCsv = args[++i].Trim('"');
            }
            else if (arg.StartsWith("--access-type=", StringComparison.OrdinalIgnoreCase))
            {
                accessType = arg["--access-type=".Length..].Trim('"');
            }
            else if (arg.Equals("--access-type", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 >= args.Length)
                {
                    error = "--access-type must be followed by an Access Type name, e.g. ImmsBCEforms.";
                    return false;
                }

                accessType = args[++i].Trim('"');
            }
            else
            {
                error = $"Unrecognised argument '{arg}'. Valid arguments are --apply, --from <path> and --access-type <name>.";
                return false;
            }
        }

        // The whole point of the flag: you cannot change live access without naming the list that was
        // reviewed. Recomputing at apply time would act on a set nobody signed off on.
        if (apply && fromCsv == null)
        {
            error = "--apply requires --from <path>. Run without arguments first to produce a report, review it, then pass it back.";
            return false;
        }

        if (fromCsv != null && !File.Exists(fromCsv))
        {
            error = $"Report file not found: {fromCsv}";
            return false;
        }

        options = new BulkRunOptions(apply, fromCsv, accessType);
        return true;
    }
}
