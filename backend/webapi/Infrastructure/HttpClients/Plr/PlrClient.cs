namespace Pidp.Infrastructure.HttpClients.Plr;

using NodaTime;

using Pidp.Extensions;
using Pidp.Models.Lookups;

public class PlrClient(HttpClient client, ILogger<PlrClient> logger) : BaseClient(client, logger), IPlrClient
{
    public async Task<string?> FindCpnAsync(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate)
    {
        var query = new
        {
            CollegeId = licenceNumber,
            Birthdate = birthdate.ToIsoDateString(),
            IdentifierTypes = MapToIdentifierTypes(collegeCode)
        };

        var result = await this.GetWithQueryParamsAsync<IEnumerable<string>>("records/cpns", query);

        if (!result.IsSuccess)
        {
            return null;
        }

        var cpns = result.Value.Distinct();

        switch (cpns.Count())
        {
            case 0:
                this.Logger.LogNoRecordsFound(query.CollegeId, query.Birthdate, query.IdentifierTypes);
                return null;
            case 1:
                return cpns.Single();
            default:
                this.Logger.LogMultipleRecordsFound(query.CollegeId, query.Birthdate, query.IdentifierTypes);
                return null;
        };
    }

    public async Task<List<PlrStatusChangeLog>> GetProcessableStatusChangesAsync(int limit = 10)
    {
        var result = await this.GetWithQueryParamsAsync<List<PlrStatusChangeLog>>("records/status-changes", new { limit });
        if (result.IsSuccess)
        {
            return result.Value;
        }
        else
        {
            this.Logger.LogPlrError(nameof(GetProcessableStatusChangesAsync));
            return [];
        }
    }

    public async Task<IEnumerable<PlrRecord>?> GetRecordsAsync(params string?[] cpns)
    {
        cpns = cpns
            .Where(cpn => !string.IsNullOrWhiteSpace(cpn))
            .ToArray();

        if (cpns.Length == 0)
        {
            return [];
        }

        var result = await this.GetWithQueryParamsAsync<IEnumerable<PlrRecord>>("records", new { Cpns = cpns });
        if (!result.IsSuccess)
        {
            return null;
        }

        return result.Value;
    }

    public async Task<bool> GetStandingAsync(string? cpn) => (await this.GetStandingsDigestAsync(cpn)).HasGoodStanding;

    public async Task<PlrStandingsDigest> GetStandingsDigestAsync(string? cpn)
    {
        if (string.IsNullOrWhiteSpace(cpn))
        {
            return PlrStandingsDigest.FromEmpty();
        }

        var records = await this.GetRecordsAsync(cpn);
        if (records == null
            || !records.Any())
        {
            return PlrStandingsDigest.FromError();
        }

        return PlrStandingsDigest.FromRecords(records);
    }

    public async Task<PlrStandingsDigest> GetAggregateStandingsDigestAsync(IEnumerable<string?> cpns)
    {
        var records = await this.GetRecordsAsync(cpns.ToArray());

        if (records == null)
        {
            return PlrStandingsDigest.FromError();
        }
        if (!records.Any())
        {
            return PlrStandingsDigest.FromEmpty();
        }

        return PlrStandingsDigest.FromRecords(records);
    }

    public async Task<bool> UpdateStatusChangeLogAsync(int statusChangeLogId)
    {
        var response = await this.PutAsync($"records/status-changes/{statusChangeLogId}/processed");
        return response.IsSuccess;
    }

    /// <summary>
    /// Returns the PLR Identifier Types(s) that correspond to the given College.
    /// </summary>
    /// <param name="collegeCode"></param>
    private static string[] MapToIdentifierTypes(CollegeCode collegeCode)
    {
        return collegeCode switch
        {
            CollegeCode.Pharmacists => [IdentifierType.Pharmacist, IdentifierType.PharmacyTech],
            CollegeCode.PhysiciansAndSurgeons => [IdentifierType.PhysiciansAndSurgeons],
            CollegeCode.NursesAndMidwives => [IdentifierType.Nurse, IdentifierType.Midwife],
            CollegeCode.NaturopathicPhysicians => [IdentifierType.NaturopathicPhysician],
            CollegeCode.DentalSurgeons => [IdentifierType.DentalSurgeon],
            CollegeCode.Optometrists => [IdentifierType.Optometrist],
            _ => []
        };
    }
}

public static partial class PlrClientLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "No Records found in PLR with CollegeId = {licenceNumber}, Birthdate = {birthdate}, and any of {identifierTypes} Identifier Types.")]
    public static partial void LogNoRecordsFound(this ILogger<BaseClient> logger, string licenceNumber, string birthdate, string[] identifierTypes);

    [LoggerMessage(2, LogLevel.Warning, "Multiple matching Records found in PLR with CollegeId = {licenceNumber}, Birthdate = {birthdate}, and any of {identifierTypes} Identifier Types.")]
    public static partial void LogMultipleRecordsFound(this ILogger<BaseClient> logger, string licenceNumber, string birthdate, string[] identifierTypes);

    [LoggerMessage(3, LogLevel.Error, "Error when calling PLR API in method {methodName}.")]
    public static partial void LogPlrError(this ILogger<BaseClient> logger, string methodName);
}
