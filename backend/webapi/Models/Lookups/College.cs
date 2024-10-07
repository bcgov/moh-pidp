namespace Pidp.Models.Lookups;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum CollegeCode
{
    PhysiciansAndSurgeons = 1,
    Pharmacists,
    NursesAndMidwives,
    NaturopathicPhysicians,
    DentalSurgeons,
    Optometrists
}

[Table("CollegeLookup")]
public class College
{
    [Key]
    public CollegeCode Code { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Acronym { get; set; } = string.Empty;
}

public class CollegeDataGenerator : ILookupDataGenerator<College>
{
    public IEnumerable<College> Generate() =>
    [
        new College { Code = CollegeCode.PhysiciansAndSurgeons,  Name = "College of Physicians and Surgeons of BC",       Acronym = "CPSBC" },
        new College { Code = CollegeCode.Pharmacists,            Name = "College of Pharmacists of BC",                   Acronym = "CPBC"  },
        new College { Code = CollegeCode.NursesAndMidwives,      Name = "BC College of Nurses and Midwives",              Acronym = "BCCNM" },
        new College { Code = CollegeCode.NaturopathicPhysicians, Name = "College of Naturopathic Physicians of BC",       Acronym = "CNPBC" },
        new College { Code = CollegeCode.DentalSurgeons,         Name = "College of Dental Surgeons of British Columbia", Acronym = "CDSBC" },
        new College { Code = CollegeCode.Optometrists,           Name = "College of Optometrists of British Columbia",    Acronym = "COBC"  },
    ];
}
