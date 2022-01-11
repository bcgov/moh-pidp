namespace PlrIntake.Models.Lookups;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(RoleType))]
public class RoleType
{
    [Key]
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}

public class RoleTypeDataGenerator : ILookupDataGenerator<RoleType>
{
    public IEnumerable<RoleType> Generate() => new[]
    {
        new RoleType { Code = "RTNM",      Name = "Nuclear Medicine Technologist"                  },
        new RoleType { Code = "RTR",       Name = "Radiation Technologist in Radiology"            },
        new RoleType { Code = "RTT",       Name = "Radiation Technologist in Therapy"              },
        new RoleType { Code = "RN",        Name = "Registered Nurse"                               },
        new RoleType { Code = "RNP",       Name = "Registered Nurse Practitioner"                  },
        new RoleType { Code = "RPN",       Name = "Registered Psychiatric Nurse"                   },
        new RoleType { Code = "RTEMG",     Name = "Registered Electromyography Technologist"       },
        new RoleType { Code = "RTMR",      Name = "Radiation Technologist in Magnetic Resonance"   },
        new RoleType { Code = "PHARM",     Name = "Pharmacist"                                     },
        new RoleType { Code = "PO",        Name = "Podiatrist"                                     },
        new RoleType { Code = "RAC",       Name = "Registered Acupuncturist"                       },
        new RoleType { Code = "REPT",      Name = "Registered Evoked Potential Technologist"       },
        new RoleType { Code = "RET",       Name = "Registered Electroencephalography Technologist" },
        new RoleType { Code = "RM",        Name = "Registered Midwife"                             },
        new RoleType { Code = "LPN",       Name = "Licensed Practical Nurse"                       },
        new RoleType { Code = "MD",        Name = "Medical Doctor"                                 },
        new RoleType { Code = "MOA",       Name = "Medical Office Assistant"                       },
        new RoleType { Code = "OPT",       Name = "Optometrist"                                    },
        new RoleType { Code = "PCP",       Name = "Primary Care Paramedic"                         },
        new RoleType { Code = "PCY",       Name = "Pharmacy"                                       },
        new RoleType { Code = "ACP",       Name = "Advanced Care Paramedic"                        },
        new RoleType { Code = "CCP",       Name = "Critical Care Paramedic"                        },
        new RoleType { Code = "DEN",       Name = "Dentist"                                        },
        new RoleType { Code = "EMR",       Name = "Emergency Medical Responder"                    },
        new RoleType { Code = "CC",        Name = "Clinical Counsellor"                            },
        new RoleType { Code = "OT",        Name = "Occupational Therapist"                         },
        new RoleType { Code = "PSYCH",     Name = "Psychologist"                                   },
        new RoleType { Code = "SW",        Name = "Social Worker"                                  },
        new RoleType { Code = "RCSW",      Name = "Registered Clinical Social Worker"              },
        new RoleType { Code = "CHIRO",     Name = "Chiropractor"                                   },
        new RoleType { Code = "PHYSIO",    Name = "Physiotherapist"                                },
        new RoleType { Code = "RMT",       Name = "Registered Massage Therapist"                   },
        new RoleType { Code = "KN",        Name = "Kinesiologist"                                  },
        new RoleType { Code = "PTECH",     Name = "Pharmacy Technician"                            },
        new RoleType { Code = "COUN",      Name = "Counsellor"                                     },
        new RoleType { Code = "MFT",       Name = "Marriage and Family Therapist"                  },
        new RoleType { Code = "RD",        Name = "Registered Dietitian"                           },
        new RoleType { Code = "PHARMTECH", Name = "PHARMTECH"                                      }
    };
}
