namespace Pidp.Infrastructure.HttpClients.FhirPlr;

using NodaTime;

using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

public class FhirPlrClient(HttpClient client, ILogger<FhirPlrClient> logger) : BaseClient(client, logger), IFhirPlrClient
{
    public async Task<string?> FindCpnAsync(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate)
    {
        var query = new
        {
            CollegeId = licenceNumber,
            Birthdate = birthdate.ToIsoDateString(),
            IdentifierTypes = MapToIdentifierTypes(collegeCode)
        };

        var result = await this.GetWithQueryParamsAsync<IEnumerable<string>>("fhir/testurl", query);

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

public static partial class FhirPlrClientLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "No Records found in PLR with CollegeId = {licenceNumber}, Birthdate = {birthdate}, and any of {identifierTypes} Identifier Types.")]
    public static partial void LogNoRecordsFound(this ILogger<BaseClient> logger, string licenceNumber, string birthdate, string[] identifierTypes);

    [LoggerMessage(2, LogLevel.Warning, "Multiple matching Records found in PLR with CollegeId = {licenceNumber}, Birthdate = {birthdate}, and any of {identifierTypes} Identifier Types.")]
    public static partial void LogMultipleRecordsFound(this ILogger<BaseClient> logger, string licenceNumber, string birthdate, string[] identifierTypes);

    [LoggerMessage(3, LogLevel.Error, "Error when calling PLR API in method {methodName}.")]
    public static partial void LogFhirPlrError(this ILogger<BaseClient> logger, string methodName);
}
