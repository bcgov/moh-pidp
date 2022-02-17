namespace Pidp.Features.Parties.ProfileStatusInternal;

using NodaTime;

using static Pidp.Features.Parties.ProfileStatus.CommandHandler;
using static Pidp.Features.Parties.ProfileStatus.Model;

public class DemographicsSection : ProfileSection
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public LocalDate Birthdate { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public DemographicsSection(ProfileDto profile)
    {
        this.FirstName = profile.FirstName;
        this.LastName = profile.LastName;
        this.Birthdate = profile.Birthdate;
        this.Email = profile.Email;
        this.Phone = profile.Phone;
    }
}

public class DemographicsSectionResolver : IProfileSectionResolver
{
    public void ComputeSection(ProfileStatus.Model profileStatus, ProfileDto profile)
    {
        var section = new DemographicsSection(profile)
        {
            StatusCode = profile.Email != null && profile.Phone != null
                ? StatusCode.Complete
                : StatusCode.Incomplete
        };

        profileStatus.Status.Add(Section.Demographics, section);
    }
}
