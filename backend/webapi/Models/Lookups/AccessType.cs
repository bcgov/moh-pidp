namespace Pidp.Models.Lookups;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum AccessTypeCode
{
    SAEforms = 1,
    HcimAccountTransfer,
    HcimEnrolment,
    DriverFitness,
    MSTeamsPrivacyOfficer,
    PrescriptionRefillEforms,
    ProviderReportingPortal
}

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
        new AccessType { Code = AccessTypeCode.SAEforms,                 Name = "Special Authority eForms"                  },
        new AccessType { Code = AccessTypeCode.HcimAccountTransfer,      Name = "HCIMWeb Account Transfer"                  },
        new AccessType { Code = AccessTypeCode.HcimEnrolment,            Name = "HCIMWeb Enrolment"                         },
        new AccessType { Code = AccessTypeCode.DriverFitness,            Name = "Driver Medical Fitness"                    },
        new AccessType { Code = AccessTypeCode.MSTeamsPrivacyOfficer,    Name = "MS Teams for Clinical Use Privacy Officer" },
        new AccessType { Code = AccessTypeCode.PrescriptionRefillEforms, Name = "Prescription Refill eForm for Pharmacists" },
        new AccessType { Code = AccessTypeCode.ProviderReportingPortal,  Name = "Provider Reporting Portal"                 },
    };
}
