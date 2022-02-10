namespace PlrIntake.Data.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlrIntake.Models;

public class IdentifierTypeConfiguration : IEntityTypeConfiguration<IdentifierType>
{
    public void Configure(EntityTypeBuilder<IdentifierType> builder)
    {
        builder.HasData(new[]
        {
            new IdentifierType { Oid = "2.16.840.1.113883.3.40.2.19", Name = "RNID"     },
            new IdentifierType { Oid = "2.16.840.1.113883.3.40.2.20", Name = "RNPID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.4.608",     Name = "RPNRC"    },
            new IdentifierType { Oid = "2.16.840.1.113883.3.40.2.14", Name = "PHID"     },
            new IdentifierType { Oid = "2.16.840.1.113883.4.454",     Name = "RACID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.3.40.2.18", Name = "RMID"     },
            new IdentifierType { Oid = "2.16.840.1.113883.3.40.2.10", Name = "LPNID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.3.40.2.4",  Name = "CPSID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.4.429",     Name = "OPTID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.3.40.2.6",  Name = "DENID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.4.363",     Name = "CCID"     },
            new IdentifierType { Oid = "2.16.840.1.113883.4.364",     Name = "OTID"     },
            new IdentifierType { Oid = "2.16.840.1.113883.4.362",     Name = "PSYCHID"  },
            new IdentifierType { Oid = "2.16.840.1.113883.4.361",     Name = "SWID"     },
            new IdentifierType { Oid = "2.16.840.1.113883.4.422",     Name = "CHIROID"  },
            new IdentifierType { Oid = "2.16.840.1.113883.4.414",     Name = "PHYSIOID" },
            new IdentifierType { Oid = "2.16.840.1.113883.4.433",     Name = "RMTID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.4.439",     Name = "KNID"     },
            new IdentifierType { Oid = "2.16.840.1.113883.4.401",     Name = "PHTID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.4.477",     Name = "COUNID"   },
            new IdentifierType { Oid = "2.16.840.1.113883.4.452",     Name = "MFTID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.4.530",     Name = "RDID"     },
            new IdentifierType { Oid = "2.16.840.1.113883.3.40.2.46", Name = "MOAID"    },
            new IdentifierType { Oid = "2.16.840.1.113883.3.40.2.44", Name = "PPID"     },
            new IdentifierType { Oid = "2.16.840.1.113883.4.538",     Name = "NDID"     }
        });
    }
}
