namespace DoWork.Services.BulkEformRoles;

using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

/// <summary>
/// Reads PLR standings from the Plr_PlrRecord table instead of the plr-intake service, so the bulk tools
/// need no PLR service running. The table lives in the same database as the Pidp tables in every
/// environment, and plr-intake's "records" endpoint is itself only a SELECT over it.
/// </summary>
public sealed class PlrRecordDbClient(PidpDbContext context) : IPlrClient
{
    private const string SelectRecords = """
        SELECT "Cpn", "IdentifierType", "CollegeId", "ProviderRoleType", "StatusCode", "StatusReasonCode", "MspId"
        FROM "Plr_PlrRecord"
        WHERE "StatusReasonCode" IS DISTINCT FROM 'REM'
          AND "Cpn" = ANY(@cpns)
        """;

    private readonly PidpDbContext context = context;

    public async Task<bool> AnyRecordsExistAsync()
    {
        await using var command = await this.CreateCommandAsync(@"SELECT count(*) FROM ""Plr_PlrRecord""");

        return Convert.ToInt64(await command.ExecuteScalarAsync(), System.Globalization.CultureInfo.InvariantCulture) > 0;
    }

    /// <summary>
    /// Mirrors IPlrClient: a non-blank CPN with no records is an error, not an empty digest, so callers
    /// treat it as "standing unknown" and leave the Party alone.
    /// </summary>
    /// <param name="cpn"></param>
    public async Task<PlrStandingsDigest> GetStandingsDigestAsync(string? cpn)
    {
        if (string.IsNullOrWhiteSpace(cpn))
        {
            return PlrStandingsDigest.FromEmpty();
        }

        var records = await this.ReadAsync([cpn]);

        return records.Count == 0
            ? PlrStandingsDigest.FromError()
            : PlrStandingsDigest.FromRecords(records);
    }

    public async Task<PlrStandingsDigest> GetAggregateStandingsDigestAsync(IEnumerable<string?> cpns)
    {
        var records = await this.ReadAsync(cpns);

        return records.Count == 0
            ? PlrStandingsDigest.FromEmpty()
            : PlrStandingsDigest.FromRecords(records);
    }

    public async Task<bool> GetStandingAsync(string? cpn) => (await this.GetStandingsDigestAsync(cpn)).HasGoodStanding;

    public Task<string?> FindCpnAsync(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate) => throw NotSupported(nameof(FindCpnAsync));

    public Task<List<PlrStatusChangeLog>> GetProcessableStatusChangesAsync(int limit = 10) => throw NotSupported(nameof(GetProcessableStatusChangesAsync));

    public async Task<IEnumerable<PlrRecord>?> GetRecordsAsync(params string?[] cpns) => await this.ReadAsync(cpns);

    public Task<bool> UpdateStatusChangeLogAsync(int statusChangeLogId) => throw NotSupported(nameof(UpdateStatusChangeLogAsync));

    private static NotSupportedException NotSupported(string member)
        => new($"{nameof(PlrRecordDbClient)} serves standings only; {member} needs the live PLR service.");

    private async Task<List<PlrRecord>> ReadAsync(IEnumerable<string?> cpns)
    {
        var distinct = cpns
            .Where(cpn => !string.IsNullOrWhiteSpace(cpn))
            .Select(cpn => cpn!)
            .Distinct()
            .ToArray();

        if (distinct.Length == 0)
        {
            return [];
        }

        await using var command = await this.CreateCommandAsync(SelectRecords);

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@cpns";
        parameter.Value = distinct;
        command.Parameters.Add(parameter);

        var records = new List<PlrRecord>();

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            records.Add(new PlrRecord
            {
                Cpn = reader.GetString(0),
                IdentifierType = ReadNullable(reader, 1),
                CollegeId = ReadNullable(reader, 2),
                ProviderRoleType = ReadNullable(reader, 3),
                StatusCode = ReadNullable(reader, 4),
                StatusReasonCode = ReadNullable(reader, 5),
                MspId = ReadNullable(reader, 6)
            });
        }

        return records;
    }

    private async Task<DbCommand> CreateCommandAsync(string sql)
    {
        var connection = this.context.Database.GetDbConnection();
        if (connection.State == ConnectionState.Closed)
        {
            await connection.OpenAsync();
        }

        var command = connection.CreateCommand();
        command.CommandText = sql;

        return command;
    }

    private static string? ReadNullable(DbDataReader reader, int ordinal) => reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
}
