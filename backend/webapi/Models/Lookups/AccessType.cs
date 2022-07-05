namespace Pidp.Models.Lookups;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum AccessTypeCode
{
    SAEforms = 1,
    HcimAccountTransfer,
    HcimEnrolment,
    DriverFitness,
    Uci
}

// This table is only for reporting/database readability; it is not used by the FE or BE of the app.
[Table("AccessTypeLookup")]
public class AccessType
{
    [Key]
    public AccessTypeCode Code { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class AccessTypeDataGenerator : ILookupDataGenerator<AccessType>
{
    public IEnumerable<AccessType> Generate() => new[]
    {
        new AccessType { Code = AccessTypeCode.SAEforms,            Name = "Special Authority eForms" },
        new AccessType { Code = AccessTypeCode.HcimAccountTransfer, Name = "HCIMWeb Account Transfer" },
        new AccessType { Code = AccessTypeCode.HcimEnrolment,       Name = "HCIMWeb Enrolment"        },
        new AccessType { Code = AccessTypeCode.DriverFitness,       Name = "Driver Medical Fitness"   },
        new AccessType { Code = AccessTypeCode.Uci,                 Name = "Fraser Health UCI"        },
    };
}
