namespace Pidp.Infrastructure.HttpClients.Plr;

using NodaTime;

using Pidp.Models.Lookups;

public class PlrRecord
{
    public string Cpn { get; set; } = string.Empty;
    public string Ipc { get; set; } = string.Empty;
    public string? CollegeId { get; set; }
    public string? IdentifierType { get; set; }
    public string? ProviderRoleType { get; set; }
    public string? StatusCode { get; set; }
    public DateTime? StatusStartDate { get; set; }
    public string? StatusReasonCode { get; set; }

    public virtual bool IsGoodStanding()
    {
        var goodStatndingReasons = new[] { "GS", "PRAC", "TEMPPER" };
        return this.StatusCode == "ACTIVE"
            && goodStatndingReasons.Contains(this.StatusReasonCode);
    }
}

public class PlrClient : BaseClient, IPlrClient
{
    public PlrClient(HttpClient client, ILogger<PlrClient> logger) : base(client, logger) { }

    public async Task<string?> FindCpn(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate)
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

    public async Task<bool?> IsGoodStanding(string? cpn)
    {
        if (string.IsNullOrWhiteSpace(cpn))
        {
            return null;
        }

        var result = await this.GetWithQueryParamsAsync<IEnumerable<PlrRecord>>("records", new { Cpn = cpn });
        if (!result.IsSuccess
            || !result.Value.Any())
        {
            return null;
        }

        return result.Value
            .Any(record => record.IsGoodStanding());
    }

    public async Task<IEnumerable<PlrRecord>?> GetRecords(string? cpn)
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

    /// <summary>
    /// Returns the PLR Identifier Types(s) that correspond to the given College.
    /// </summary>
    /// <param name="collegeCode"></param>
    private static string[] MapToIdentifierTypes(CollegeCode collegeCode)
    {
        return collegeCode switch
        {
            CollegeCode.Pharmacists => new[] { "PHID", "PHTID" },
            CollegeCode.PhysiciansAndSurgeons => new[] { "CPSID" },
            CollegeCode.NursesAndMidwives => new[] { "RNID", "RMID" },
            CollegeCode.NaturopathicPhysicians => new[] { "NDID" },
            CollegeCode.DentalSurgeons => new[] { "DENID" },
            CollegeCode.Optometrists => new[] { "OPTID" },
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
