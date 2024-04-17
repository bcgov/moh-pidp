namespace Pidp.Models.Lookups;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum AccessTypeCode
{
    SAEforms = 1,
    HcimAccountTransfer,
    HcimEnrolment, // Currently Unused
    DriverFitness,
    MSTeamsPrivacyOfficer,
    PrescriptionRefillEforms,
    ProviderReportingPortal,
    MSTeamsClinicMember,
    UserAccessAgreement,
    ImmsBCEforms,
    EdrdEforms
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
        new AccessType { Code = AccessTypeCode.SAEforms,                 Name = "Special Authority eForms"                    },
        new AccessType { Code = AccessTypeCode.HcimAccountTransfer,      Name = "HCIMWeb Account Transfer"                    },
        new AccessType { Code = AccessTypeCode.HcimEnrolment,            Name = "HCIMWeb Enrolment"                           },
        new AccessType { Code = AccessTypeCode.DriverFitness,            Name = "Driver Medical Fitness"                      },
        new AccessType { Code = AccessTypeCode.MSTeamsPrivacyOfficer,    Name = "MS Teams for Clinical Use - Privacy Officer" },
        new AccessType { Code = AccessTypeCode.PrescriptionRefillEforms, Name = "Prescription Refill eForm for Pharmacists"   },
        new AccessType { Code = AccessTypeCode.ProviderReportingPortal,  Name = "Provider Reporting Portal"                   },
        new AccessType { Code = AccessTypeCode.MSTeamsClinicMember,      Name = "MS Teams for Clinical Use - Clinic Member"   },
        new AccessType { Code = AccessTypeCode.UserAccessAgreement,      Name = "Access Harmonization User Access Agreement"  },
        new AccessType { Code = AccessTypeCode.ImmsBCEforms,             Name = "Immunization Entry eForm"                    },
        new AccessType { Code = AccessTypeCode.EdrdEforms,               Name = "EDRD eForm"                                  },
    };
}
