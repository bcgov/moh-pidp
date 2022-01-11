namespace PlrIntake.Models.Lookups;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(StatusReason))]
public class StatusReason
{
    [Key]
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}

public class StatusReasonDataGenerator : ILookupDataGenerator<StatusReason>
{
    public IEnumerable<StatusReason> Generate() => new[]
    {
        new StatusReason { Code = "OOP",       Name = "Out of Province"                },
        new StatusReason { Code = "ORG",       Name = "Organization Provider"          },
        new StatusReason { Code = "DEF",       Name = "Deferred"                       },
        new StatusReason { Code = "DEN",       Name = "Licensed Denied"                },
        new StatusReason { Code = "ERSRES",    Name = "Erased by Resolution"           },
        new StatusReason { Code = "GS",        Name = "Good Standing"                  },
        new StatusReason { Code = "HON",       Name = "Honorary"                       },
        new StatusReason { Code = "INNONPRAC", Name = "Initial Non Practicing"         },
        new StatusReason { Code = "MIS",       Name = "Missionary"                     },
        new StatusReason { Code = "NR",        Name = "Non-resident"                   },
        new StatusReason { Code = "OPEN",      Name = "Open"                           },
        new StatusReason { Code = "PRAC",      Name = "Practising"                     },
        new StatusReason { Code = "REM",       Name = "Removed"                        },
        new StatusReason { Code = "RESDISC",   Name = "Resigned - disciplinary action" },
        new StatusReason { Code = "SPE",       Name = "Special Registry"               },
        new StatusReason { Code = "SUS",       Name = "Suspended"                      },
        new StatusReason { Code = "TEMPPER",   Name = "Temporary Permit"               },
        new StatusReason { Code = "TI",        Name = "Temporary Inactive"             },
        new StatusReason { Code = "TSF",       Name = "Transfer"                       },
        new StatusReason { Code = "UNK",       Name = "Unknown"                        },
        new StatusReason { Code = "VW",        Name = "Voluntary Withdrawal"           },
        new StatusReason { Code = "ASSOC",     Name = "Associate"                      },
        new StatusReason { Code = "AU",        Name = "Address Unknown"                },
        new StatusReason { Code = "CLOSED",    Name = "Closed"                         },
        new StatusReason { Code = "NONPRAC",   Name = "Non Practicing"                 },
        new StatusReason { Code = "LAP",       Name = "License Lapsed on Request"      },
        new StatusReason { Code = "LTP",       Name = "Left the Province"              },
        new StatusReason { Code = "NONPAY",    Name = "Non Payment of Fee"             },
        new StatusReason { Code = "RET",       Name = "Retired"                        },
        new StatusReason { Code = "DEC",       Name = "Deceased"                       },
        new StatusReason { Code = "MEDSTUD",   Name = "Medical Student"                }
    };
}
