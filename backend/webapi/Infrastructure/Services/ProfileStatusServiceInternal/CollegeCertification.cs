namespace Pidp.Infrastructure.Services.ProfileStatusServiceInternal;

using System.Collections.Generic;

using Pidp.Models.Lookups;
using Pidp.Models.ProfileStatus;

public class CollegeCertificationSectionResolver : IProfileSectionResolver
{
    public IProfileSection ResolveSection(ProfileStatusDto profile) => new CollegeCertificationSection(profile);
}

public class CollegeCertificationSection : IProfileSection
{
    public string Name => Section.CollegeCertification.Name;
    public HashSet<Alert> Alerts { get; } = new();
    public StatusCode StatusCode { get; private set; }
    public CollegeCode? CollegeCode { get; }
    public string? LicenceNumber { get; }

    public CollegeCertificationSection(ProfileStatusDto profile)
    {
        this.CollegeCode = profile.CollegeCode;
        this.LicenceNumber = profile.LicenceNumber;
        this.SetStatusAndAlerts(profile);
    }

    private void SetStatusAndAlerts(ProfileStatusDto profile)
    {
        if (!profile.GetSection(Section.Demographics).IsComplete)
        {
            this.StatusCode = StatusCode.Locked;
            return;
        }

        if (profile.CollegeCode == null || profile.LicenceNumber == null)
        {
            this.StatusCode = StatusCode.Incomplete;
            return;
        }

        if (profile.Ipc == null
            || profile.PlrRecordStatus == null)
        {
            this.StatusCode = StatusCode.Error;
            this.Alerts.Add(Alert.TransientError);
            return;
        }

        if (!profile.PlrRecordStatus.IsGoodStanding())
        {
            this.StatusCode = StatusCode.Error;
            this.Alerts.Add(Alert.PlrBadStanding);
            return;
        }

        this.StatusCode = StatusCode.Complete;
    }
}
