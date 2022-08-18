namespace Pidp.Infrastructure.HttpClients.Plr;

using NodaTime;

using Pidp.Models.Lookups;

public interface IPlrClient
{
    /// <summary>
    /// Searches PLR for Records matching the user's College information and Birthdate, and Returns the single CPN of those records.
    /// Returns null on an error, no records are found, or if multiple matching records are found with different CPNs.
    /// </summary>
    /// <param name="collegeCode"></param>
    /// <param name="licenceNumber"></param>
    /// <param name="birthdate"></param>
    Task<string?> FindCpnAsync(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate);

    /// <summary>
    /// Fetches the PLR record(s) corresponding to the given CPN.
    /// Returns null on an error.
    /// </summary>
    /// <param name="cpn"></param>
    Task<IEnumerable<PlrRecord>?> GetRecordsAsync(string cpn);

    /// <summary>
    /// Returns true if the user has at least one Record in good standing in PLR (and there are no errors).
    /// Convience method for (await GetStandingsDigestAsync(cpn)).HasGoodStanding.
    /// </summary>
    /// <param name="cpn"></param>
    Task<bool> GetStandingAsync(string? cpn);

    /// <summary>
    /// Creates a summary of the status of all PLR Records for the given CPN.
    /// The digest indicates an error on HTTP failure or if the CPN is not null but finds no Records.
    /// </summary>
    /// <param name="cpn"></param>
    Task<PlrStandingsDigest> GetStandingsDigestAsync(string? cpn);
}
