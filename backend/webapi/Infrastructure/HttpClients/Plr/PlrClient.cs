namespace Pidp.Infrastructure.HttpClients.Plr;

using NodaTime;

using Pidp.Extensions;
using Pidp.Models.Lookups;

public class PlrClient : BaseClient, IPlrClient
{
    public PlrClient(HttpClient client, ILogger<PlrClient> logger) : base(client, logger) { }

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

    public async Task<IEnumerable<PlrRecord>?> GetRecordsAsync(params string?[] cpns)
    {
        cpns = cpns
            .Where(cpn => !string.IsNullOrWhiteSpace(cpn))
            .ToArray();

        if (!cpns.Any())
        {
            return Enumerable.Empty<PlrRecord>();
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

    public async Task<IList<PlrStatusChangeLog>> GetStatusChangeToPocess()
    {
        var result = await this.GetAsync<IList<PlrStatusChangeLog>>("records/status-changes");
        return result.Value;
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

    public async Task<bool> SetStatusChangeLogToProcessed(int statusChangeLogId)
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
            CollegeCode.Pharmacists => new string[] { IdentifierType.Pharmacist, IdentifierType.PharmacyTech },
            CollegeCode.PhysiciansAndSurgeons => new string[] { IdentifierType.PhysiciansAndSurgeons },
            CollegeCode.NursesAndMidwives => new string[] { IdentifierType.Nurse, IdentifierType.Midwife },
            CollegeCode.NaturopathicPhysicians => new string[] { IdentifierType.NaturopathicPhysician },
            CollegeCode.DentalSurgeons => new string[] { IdentifierType.DentalSurgeon },
            CollegeCode.Optometrists => new string[] { IdentifierType.Optometrist },
            _ => Array.Empty<string>()
        };
    }
}

public static partial class PlrClientLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "No Records found in PLR with CollegeId = {licenceNumber}, Birthdate = {birthdate}, and any of {identifierTypes} Identifier Types.")]
    public static partial void LogNoRecordsFound(this ILogger logger, string licenceNumber, string birthdate, string[] identifierTypes);

    [LoggerMessage(2, LogLevel.Warning, "Multiple matching Records found in PLR with CollegeId = {licenceNumber}, Birthdate = {birthdate}, and any of {identifierTypes} Identifier Types.")]
    public static partial void LogMultipleRecordsFound(this ILogger logger, string licenceNumber, string birthdate, string[] identifierTypes);
}
