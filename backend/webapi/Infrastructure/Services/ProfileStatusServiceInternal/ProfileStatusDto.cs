namespace Pidp.Infrastructure.Services.ProfileStatusServiceInternal;

using NodaTime;
using System.Security.Claims;

using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;
using Pidp.Models.ProfileStatus;

public class ProfileStatusDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public LocalDate Birthdate { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public CollegeCode? CollegeCode { get; set; }
    public string? LicenceNumber { get; set; }
    public string? Ipc { get; set; }
    public IEnumerable<AccessType> CompletedEnrolments { get; set; } = Enumerable.Empty<AccessType>();

    // Computed after projection
    public PlrRecordStatus? PlrRecordStatus { get; set; }
    public ClaimsPrincipal? User { get; set; }

    private Dictionary<string, IProfileSection> ResolvedSections { get; } = new();

    public IProfileSection GetSection(Section section)
    {
        if (this.ResolvedSections.TryGetValue(section.Name, out var resolved))
        {
            return resolved;
        }

        resolved = section.Resolver.ResolveSection(this);
        this.ResolvedSections.Add(section.Name, resolved);
        return resolved;
    }

    public IEnumerable<IProfileSection> GetSections(params Section[] sections)
    {
        foreach (var section in sections)
        {
            yield return this.GetSection(section);
        }
    }
}
