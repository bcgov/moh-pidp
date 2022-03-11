namespace Pidp.Infrastructure.Services.ProfileStatusServiceInternal;

using System;
using System.Collections.Generic;
using Pidp.Models;
using Pidp.Models.ProfileStatus;

public class SAEformsSectionResolver : IProfileSectionResolver
{
    public IProfileSection ResolveSection(ProfileStatusDto profile) => new SAEformsSection(profile);
}

public class SAEformsSection : IProfileSection
{
    public string Name => Section.SAEforms.Name;
    public HashSet<Alert> Alerts { get; } = new();
    public StatusCode StatusCode { get; private set; }

    public SAEformsSection(ProfileStatusDto profile) => this.SetStatus(profile);

    private void SetStatus(ProfileStatusDto profile)
    {
        if (profile.CompletedEnrolments.Contains(AccessType.SAEforms))
        {
            this.StatusCode = StatusCode.Complete;
            return;
        }

        if (!profile.GetSection(Section.Demographics).IsComplete
            || !profile.GetSection(Section.CollegeCertification).IsComplete
            || profile.PlrRecordStatus == null
            || !profile.PlrRecordStatus.IsGoodStanding())
        {
            this.StatusCode = StatusCode.Locked;
            return;
        }

        this.StatusCode = StatusCode.Incomplete;
    }
}
