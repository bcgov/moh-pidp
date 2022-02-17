namespace Pidp.Infrastructure.HttpClients.Plr;

using NodaTime;

using Pidp.Models.Lookups;

public interface IPlrClient
{
    /// <summary>
    /// Returns the IPC of the PLR record matching the given certification data and birthdate.
    /// Returns null on an error, if no record is found, or if multiple records are found.
    /// </summary>
    /// <param name="licenceNumber"></param>
    /// <param name="collegeCode"></param>
    /// <param name="birthdate"></param>
    Task<string?> GetPlrRecord(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate);

    /// <summary>
    /// Returns the Status of the PLR record matching the given Ipc.
    /// Returns null on an error or if no record is found.
    /// </summary>
    /// <param name="ipc"></param>
    Task<PlrRecordStatus?> GetRecordStatus(string? ipc);
}
