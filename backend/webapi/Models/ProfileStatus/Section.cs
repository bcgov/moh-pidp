namespace Pidp.Models.ProfileStatus;

using Pidp.Infrastructure.Services.ProfileStatusServiceInternal;

public record Section
{
    // public static readonly Section CareSetting = new("careSetting", new { });
    public static readonly Section CollegeCertification = new("collegeCertification", new CollegeCertificationSectionResolver());
    public static readonly Section Demographics = new("demographics", new DemographicsSectionResolver());
    // public static readonly Section Hcim = new("hcim", new { });
    public static readonly Section SAEforms = new("saEforms", new SAEformsSectionResolver());

    public static readonly List<Section> AllSections = new();

    public string Name { get; }
    public IProfileSectionResolver Resolver { get; }

    private Section(string name, IProfileSectionResolver sectionResolver)
    {
        this.Name = name;
        this.Resolver = sectionResolver;
        AllSections.Add(this);
    }
}
