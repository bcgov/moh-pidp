namespace Pidp.Infrastructure.HttpClients.Plr;

using NodaTime;

using Pidp.Models.Lookups;

public class PlrClient : BaseClient, IPlrClient
{
    public PlrClient(HttpClient client, ILogger<PlrClient> logger) : base(client, logger) { }

    public async Task<string?> FindCpnAsync(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate)
    {
        var query = new
        {
            CollegeId = licenceNumber,
            Birthdate = birthdate.ToString(),
            IdentifierTypes = MapToIdentifierTypes(collegeCode)
        };
        var result = await this.GetWithQueryParamsAsync<IEnumerable<PlrRecord>>("records", query);

        if (!result.IsSuccess)
        {
            return null;
        }

        var cpns = result.Value
            .Select(record => record.Cpn)
            .Distinct();

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

    public async Task<IEnumerable<PlrRecord>?> GetRecordsAsync(string? cpn)
    {
        if (string.IsNullOrWhiteSpace(cpn))
        {
            return Enumerable.Empty<PlrRecord>();
        }

        var result = await this.GetWithQueryParamsAsync<IEnumerable<PlrRecord>>("records", new { Cpn = cpn });
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

        var result = await this.GetWithQueryParamsAsync<IEnumerable<PlrRecord>>("records", new { Cpn = cpn });
        if (!result.IsSuccess
            || !result.Value.Any())
        {
            return PlrStandingsDigest.FromError();
        }

        return PlrStandingsDigest.FromRecords(result.Value);
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
