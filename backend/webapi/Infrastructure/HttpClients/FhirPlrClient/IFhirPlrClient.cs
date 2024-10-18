namespace Pidp.Infrastructure.HttpClients.FhirPlr;

using NodaTime;

using Pidp.Models.Lookups;

public interface IFhirPlrClient
{
    /// <summary>
    /// Searches PLR for Records matching the user's College information and Birthdate, and Returns the single CPN of those records.
    /// Returns null on an error, no records are found, or if multiple matching records are found with different CPNs.
    /// </summary>
    /// <param name="collegeCode"></param>
    /// <param name="licenceNumber"></param>
    /// <param name="birthdate"></param>
    Task<string?> FindCpnAsync(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate);

}
