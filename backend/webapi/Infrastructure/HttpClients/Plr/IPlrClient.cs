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
    /// Get a list of PLR status changes to be processed.
    /// </summary>
    /// <returns></returns>
    Task<List<PlrStatusChangeLog>> GetProcessableStatusChangesAsync(int limit = 10);

    /// <summary>
    /// Fetches the PLR record(s) corresponding to the given CPN(s).
    /// Returns null on an error.
    /// </summary>
    /// <param name="cpns"></param>
    Task<IEnumerable<PlrRecord>?> GetRecordsAsync(params string?[] cpns);

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

    /// Creates a summary of the status of all PLR Records for all of the given CPNs.
    /// The digest indicates an error on HTTP failure.
    /// </summary>
    /// <param name="cpns"></param>
    Task<PlrStandingsDigest> GetAggregateStandingsDigestAsync(IEnumerable<string?> cpns);

    /// <summary>
    /// Update the Status Change Log to "processed".
    /// </summary>
    /// <param name="statusChangeLogId"></param>
    /// <returns></returns>
    Task<bool> UpdateStatusChangeLogAsync(int statusChangeLogId);
}
