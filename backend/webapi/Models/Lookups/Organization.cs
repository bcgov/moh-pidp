namespace Pidp.Models.Lookups;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum OrganizationCode
{
    HealthAuthority = 1,
    BcGovernmentMinistry,
    Maximus,
    ICBC,
    Other
}

[Table("OrganizationLookup")]
public class Organization
{
    [Key]
    public OrganizationCode Code { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class OrganizationDataGenerator : ILookupDataGenerator<Organization>
{
    public IEnumerable<Organization> Generate() => new[]
    {
        new Organization { Code = OrganizationCode.HealthAuthority,      Name = "Health Authority"       },
        new Organization { Code = OrganizationCode.BcGovernmentMinistry, Name = "BC Government Ministry" },
        new Organization { Code = OrganizationCode.Maximus,              Name = "Maximus"                },
        new Organization { Code = OrganizationCode.ICBC,                 Name = "ICBC"                   },
        new Organization { Code = OrganizationCode.Other,                Name = "Other"                  }
    };
}
