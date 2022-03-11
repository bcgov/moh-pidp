namespace Pidp.Infrastructure.Services.ProfileStatusServiceInternal;

using NodaTime;
using System.Collections.Generic;

using Pidp.Models.ProfileStatus;

public class DemographicsSectionResolver : IProfileSectionResolver
{
    public IProfileSection ResolveSection(ProfileStatusDto profile)
    {
        return new DemographicsSection(profile,
            statusCode: profile.Email != null && profile.Phone != null
                ? StatusCode.Complete
                : StatusCode.Incomplete);
    }
}

public class DemographicsSection : IProfileSection
{
    public string Name => Section.Demographics.Name;
    public HashSet<Alert> Alerts => new();
    public StatusCode StatusCode { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public LocalDate Birthdate { get; }
    public string? Email { get; }
    public string? Phone { get; }

    public DemographicsSection(ProfileStatusDto profile, StatusCode statusCode)
    {
        this.StatusCode = statusCode;
        this.FirstName = profile.FirstName;
        this.LastName = profile.LastName;
        this.Birthdate = profile.Birthdate;
        this.Email = profile.Email;
        this.Phone = profile.Phone;
    }
}
