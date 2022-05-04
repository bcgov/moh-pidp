namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum AccessType
{
    SAEforms = 1,
    HcimAccountTransfer,
    HcimEnrolment
}

[Table(nameof(AccessRequest))]
public class AccessRequest : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public Instant RequestedOn { get; set; }

    public AccessType AccessType { get; set; }
}

[Table(nameof(HcimAccountTransfer))]
public class HcimAccountTransfer : AccessRequest
{
    public string LdapUsername { get; set; } = string.Empty;
}

[Table(nameof(HcimEnrolment))]
public class HcimEnrolment : AccessRequest
{
    public bool ManagesTasks { get; set; }
    public bool ModifiesPhns { get; set; }
    public bool RecordsNewborns { get; set; }
    public bool SearchesIdentifiers { get; set; }
}
