namespace Pidp.Features.Parties.ProfileStatusInternal;

using Pidp.Models;
using static Pidp.Features.Parties.ProfileStatus.CommandHandler;
using static Pidp.Features.Parties.ProfileStatus.Model;

public class SAEformsSection : ProfileSection
{
}

public class SAEformsSectionResolver : IProfileSectionResolver
{
    public void ComputeSection(ProfileStatus.Model profileStatus, ProfileDto profile)
    {
        var section = new SAEformsSection()
        {
            StatusCode = ComputeStatus(profileStatus, profile)
        };

        profileStatus.Status.Add(Section.SAEforms, section);
    }

    private static StatusCode ComputeStatus(ProfileStatus.Model profileStatus, ProfileDto profile)
    {
        if (profile.CompletedEnrolments.Contains(AccessType.SAEforms))
        {
            return StatusCode.Complete;
        }

        if (profileStatus.GetSectionStatus(Section.Demographics) != StatusCode.Complete
            || profileStatus.GetSectionStatus(Section.CollegeCertification) != StatusCode.Complete
            || profile.PlrRecordStatus == null
            || !profile.PlrRecordStatus.IsGoodStanding())
        {
            return StatusCode.Locked;
        }

        return StatusCode.Incomplete;
    }
}
