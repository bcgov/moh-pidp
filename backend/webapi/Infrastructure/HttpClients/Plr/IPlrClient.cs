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
    Task<string?> FindCpn(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate);

    /// <summary>
    /// Checks PLR to see if the given CPN has at least one Record in "good standing".
    /// Returns null on an error or if no records are found.
    /// </summary>
    /// <param name="cpn"></param>
    Task<bool?> IsGoodStanding(string? cpn);

    /// <summary>
    /// Fetches the PLR record(s) corresponding to the given CPN.
    /// Returns null on an error.
    /// </summary>
    /// <param name="cpn"></param>
    Task<IEnumerable<PlrRecord>?> GetRecords(string cpn);
}
