using System.ComponentModel;

namespace Pidp.Models.Lookups;

/// <summary>
/// These roles come from IMMS1 and were persisted in bcprovider. IMMS2 (launch Sept 2026) uses 'enduser' (for clinican and clerk) and 'lead' (for admin)
/// </summary>
public enum PharmacyRole
{
    [Description("IMMS1 Clinician")]
    Clinician = 1,
    [Description("IMMS1 Clerk")]
    Clerk = 2,
    [Description("IMMS1 Admin")]
    Admin = 3,
    [Description("IMMS2 End User")]
    EndUser = 4,
    [Description("IMMS2 Lead")]
    Lead = 5,
    [Description("Not assigned in IMMS")]
    Unknown = 99
}