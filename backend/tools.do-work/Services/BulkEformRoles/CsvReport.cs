namespace DoWork.Services.BulkEformRoles;

using System.Globalization;
using System.Text;

/// <summary>
/// Writes a timestamped report beside the executable, and reads one back so an apply run can be limited
/// to the rows a human reviewed. Rows are flushed as they are written, so a run that fails part way
/// through still leaves a record of what it did.
/// </summary>
public sealed class CsvReport : IDisposable
{
    /// <summary>
    /// Names which tool produced the report. Written into every row rather than inferred from the file
    /// name, which a reviewer is free to change, so a run can refuse a report meant for another tool.
    /// </summary>
    public const string KindColumn = "Report";

    public const string OutcomeColumn = "Outcome";

    public const string AccessTypeColumn = "AccessType";

    public const string PartyIdColumn = "PartyId";

    private readonly StreamWriter writer;
    private readonly string kind;

    public string FilePath { get; }

    /// <param name="kind">Identifies the producing tool. Used as the file name prefix and written to every row.</param>
    /// <param name="headers">Columns after the <see cref="KindColumn"/>, which is added automatically.</param>
    public CsvReport(string kind, params string[] headers)
    {
        this.kind = kind;
        this.FilePath = Path.Combine(
            AppContext.BaseDirectory,
            $"{kind}-{DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture)}.csv");

        this.writer = new StreamWriter(this.FilePath, append: false, Encoding.UTF8) { AutoFlush = true };
        this.writer.WriteLine(string.Join(',', headers.Prepend(KindColumn).Select(Escape)));
    }

    public void WriteRow(params object?[] values) => this.writer.WriteLine(string.Join(',', values.Prepend(this.kind).Select(Escape)));

    public void Dispose() => this.writer.Dispose();

    /// <summary>
    /// Keyed by column name and tolerant of unknown columns, so a report survives being opened, sorted
    /// and re-saved in Excel.
    /// </summary>
    /// <param name="path"></param>
    public static List<Dictionary<string, string>> Read(string path)
    {
        var rows = new List<Dictionary<string, string>>();

        using var reader = new StreamReader(path);

        var headers = ReadRecord(reader);
        if (headers == null)
        {
            return rows;
        }

        while (ReadRecord(reader) is { } fields)
        {
            if (fields.Count == 1 && string.IsNullOrWhiteSpace(fields[0]))
            {
                continue;
            }

            var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < headers.Count && i < fields.Count; i++)
            {
                row[headers[i]] = fields[i];
            }

            rows.Add(row);
        }

        return rows;
    }

    public static HashSet<int> PartyIds(IEnumerable<Dictionary<string, string>> rows)
    {
        return rows
            .Select(row => row.TryGetValue(PartyIdColumn, out var raw)
                && int.TryParse(raw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var partyId)
                    ? partyId
                    : (int?)null)
            .Where(partyId => partyId != null)
            .Select(partyId => partyId!.Value)
            .ToHashSet();
    }

    /// <summary>
    /// The distinct non-blank values of one column. Empty when the column is absent entirely, which
    /// callers treat as a failure rather than as consent.
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="column"></param>
    public static HashSet<string> DistinctValues(IEnumerable<Dictionary<string, string>> rows, string column)
    {
        return rows
            .Select(row => row.GetValueOrDefault(column))
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value!.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public static bool HasValue(Dictionary<string, string> row, string column, string expected)
        => row.GetValueOrDefault(column, string.Empty).Trim().Equals(expected, StringComparison.OrdinalIgnoreCase);

    private static string Escape(object? value)
    {
        var text = Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;

        return text.AsSpan().IndexOfAny(",\"\r\n") >= 0
            ? $"\"{text.Replace("\"", "\"\"", StringComparison.Ordinal)}\""
            : text;
    }

    /// <summary>
    /// Reads one record, which may span several physical lines when a field is quoted.
    /// </summary>
    /// <param name="reader"></param>
    private static List<string>? ReadRecord(StreamReader reader)
    {
        if (reader.EndOfStream)
        {
            return null;
        }

        var fields = new List<string>();
        var field = new StringBuilder();
        var inQuotes = false;

        while (true)
        {
            var read = reader.Read();
            if (read == -1)
            {
                fields.Add(field.ToString());
                return fields;
            }

            var c = (char)read;

            if (inQuotes)
            {
                if (c != '"')
                {
                    field.Append(c);
                }
                else if (reader.Peek() == '"')
                {
                    reader.Read();
                    field.Append('"');
                }
                else
                {
                    inQuotes = false;
                }

                continue;
            }

            switch (c)
            {
                case '"':
                    inQuotes = true;
                    break;
                case ',':
                    fields.Add(field.ToString());
                    field.Clear();
                    break;
                case '\r':
                    break;
                case '\n':
                    fields.Add(field.ToString());
                    return fields;
                default:
                    field.Append(c);
                    break;
            }
        }
    }
}
