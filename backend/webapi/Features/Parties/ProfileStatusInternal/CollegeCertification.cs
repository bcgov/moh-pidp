
namespace Pidp.Features.Parties.ProfileStatusInternal;

using Pidp.Models.Lookups;
using static Pidp.Features.Parties.ProfileStatus.CommandHandler;
using static Pidp.Features.Parties.ProfileStatus.Model;

public class CollegeCertificationSection : ProfileSection
{
    public CollegeCode? CollegeCode { get; set; }
    public string? LicenceNumber { get; set; }

    public CollegeCertificationSection(ProfileDto profile)
    {
        this.CollegeCode = profile.CollegeCode;
        this.LicenceNumber = profile.LicenceNumber;
    }
}

public class CollegeCertificationSectionResolver : IProfileSectionResolver
{
    public void ComputeSection(ProfileStatus.Model profileStatus, ProfileDto profile)
    {
        var section = new CollegeCertificationSection(profile)
        {
            StatusCode = ComputeStatus(profileStatus, profile)
        };

        profileStatus.Status.Add(Section.CollegeCertification, section);
    }

    private static StatusCode ComputeStatus(ProfileStatus.Model profileStatus, ProfileDto profile)
    {
        if (!profileStatus.SectionIsComplete(Section.Demographics))
        {
            return StatusCode.Locked;
        }

        if (profile.CollegeCode == null || profile.LicenceNumber == null)
        {
            return StatusCode.Incomplete;
        }

        if (profile.Ipc == null
            || profile.PlrRecordStatus == null
            || !profile.PlrRecordStatus.IsGoodStanding())
        {
            return StatusCode.Error;
        }

        return StatusCode.Complete;
    }
}
