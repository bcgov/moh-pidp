namespace Pidp.Models.Lookups;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum CollegeCode
{
    PhysiciansAndSurgeons = 1,
    Pharmacists,
    NursesAndMidwives
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
    public IEnumerable<College> Generate() => new[]
    {
        new College { Code = CollegeCode.PhysiciansAndSurgeons, Name = "College of Physicians and Surgeons of BC", Acronym = "CPSBC" },
        new College { Code = CollegeCode.Pharmacists,           Name = "College of Pharmacists of BC",             Acronym = "CPBC"  },
        new College { Code = CollegeCode.NursesAndMidwives,     Name = "BC College of Nurses and Midwives",        Acronym = "BCCNM" },
    };
}
