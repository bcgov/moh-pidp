namespace Pidp.Models.Lookups;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum HealthAuthorityCode
{
    ProvincialHealthServicesAuthority = 1,
    VancouverIslandHealthAuthority,
    VancouverCoastalHealthAuthority,
    FraserHealthAuthority,
    InteriorHealthAuthority,
    NorthernHealthAuthority,
    FirstNationsHealthAuthority
}

[Table("HealthAuthorityLookup")]
public class HealthAuthority
{
    [Key]
    public HealthAuthorityCode Code { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class HealthAuthorityDataGenerator : ILookupDataGenerator<HealthAuthority>
{
    public IEnumerable<HealthAuthority> Generate() => new[]
    {

        new HealthAuthority { Code = HealthAuthorityCode.ProvincialHealthServicesAuthority, Name = "Provincial Health Services Authority" },
        new HealthAuthority { Code = HealthAuthorityCode.VancouverIslandHealthAuthority,    Name = "Vancouver Island Health Authority"    },
        new HealthAuthority { Code = HealthAuthorityCode.VancouverCoastalHealthAuthority,   Name = "Vancouver Coastal Health Authority"   },
        new HealthAuthority { Code = HealthAuthorityCode.FraserHealthAuthority,             Name = "Fraser Health Authority"              },
        new HealthAuthority { Code = HealthAuthorityCode.InteriorHealthAuthority,           Name = "Interior Health Authority"            },
        new HealthAuthority { Code = HealthAuthorityCode.NorthernHealthAuthority,           Name = "Northern Health Authority"            },
        new HealthAuthority { Code = HealthAuthorityCode.FirstNationsHealthAuthority,       Name = "First Nations Health Authority"       }
    };
}
