namespace Pidp.Infrastructure.Services;

using Pidp.Models.ProfileStatus;

public interface IProfileStatusService
{
    /// <summary>
    /// Computes the Status Code, Alerts, and Properties of all supplied Sections, resolving dependant Sections as required.
    /// </summary>
    /// <param name="partyId"></param>
    /// <param name="sections"></param>
    Task<IEnumerable<IProfileSection>> CompileSectionsAsync(int partyId, params Section[] sections);

    /// <summary>
    /// Computes the Status of a single Section, resolving dependant Sections as required.
    /// </summary>
    /// <param name="partyId"></param>
    /// <param name="section"></param>
    Task<StatusCode> ComputeStatusAsync(int partyId, Section section);
}


